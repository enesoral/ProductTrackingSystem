using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using ProductTrackingSystem.Models.Entity;

namespace ProductTrackingSystem.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ProductTrackingEntities db = new ProductTrackingEntities();

        public ActionResult Index(string searching, int? page, int? ddlListType, int? ddlMonth)
        {
            ViewBag.CurrentListType = ddlListType;
            ViewBag.CurrentMonth = ddlMonth;
            ViewBag.CurrentSearching = searching;

            List<Product> products;

            switch (ddlListType)
            {
                case 2:
                    products = new List<Product>(db.Products.Where(p => !p.is_active));
                    break;
                case 3:
                    products = new List<Product>((IEnumerable<Product>)db.Products.SqlQuery(
                        "SELECT * FROM Products WHERE DATEDIFF(day, GETDATE(), expiration_date) <= {0} AND DATEDIFF(day, GETDATE(), expiration_date) >= 0", ddlMonth * 30));
                    break;
                case 4:
                    products = new List<Product>((IEnumerable<Product>)db.Products.SqlQuery(
                        "SELECT * FROM Products WHERE DATEDIFF(day, GETDATE(), expiration_date) <= 0"));
                    break;
                case 5:
                    products = new List<Product>((IEnumerable<Product>)db.Products.SqlQuery(
                        "SELECT * FROM Products WHERE MONTH(renovation_date) <= 6 AND YEAR(renovation_date) = YEAR(GETDATE())"));
                    break;
                case 6:
                    products = new List<Product>((IEnumerable<Product>)db.Products.SqlQuery(
                        "SELECT * FROM Products WHERE MONTH(renovation_date) > 6 AND YEAR(renovation_date) = YEAR(GETDATE())"));
                    break;
                case 7:
                    products = new List<Product>(db.Products.Where(p => p.usage_type));
                    break;
                case 8:
                    products = new List<Product>(db.Products.Where(p => !p.usage_type));
                    break;
                default:
                    products = new List<Product>(db.Products.Where(p => p.is_active));
                    break;
            }

            if (!String.IsNullOrEmpty(searching))
                if (!searching.Trim().Equals(""))
                    products = new List<Product>(db.Products.Where(x => x.name.Contains(searching.Trim()) || x.specification_name.Contains(searching.Trim())));

            int pageNumber = (page ?? 1);
            PagedList<Product> model = new PagedList<Product>(products.ToList(), pageNumber, 5);

            return View(model);
        }

        public ActionResult Detail(int? id)
        {
            id = (id ?? 1);
            var model = db.Products.ToList().FirstOrDefault(x => x.id == id);
            if (model == null)
                return RedirectToAction("Index");

            return PartialView("Detail", model);
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        public ActionResult EditProduct(int? id)
        {
            id = (id ?? 1);
            var model = db.Products.ToList().FirstOrDefault(x => x.id == id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveProduct(FormCollection form)
        {
            if (String.IsNullOrEmpty(form["id"]))
            {
                Product product = new Product();
                product = setProductFromForm(product, form, true);
                db.Products.Add(product);
                TempData["message"] = "Added";
            }
            else
            {
                var oldProduct = db.Products.Find(Convert.ToInt32(form["id"]));
                setProductFromForm(oldProduct, form, false);
                TempData["message"] = "Edited";
            }
            
            db.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Product");
            }

            Product product = db.Products.Find(id); 
            db.Products.Remove(product);
            TempData["message"] = "Deleted";
            db.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

        private DateTime formatToDate(string strDate)
        {
            string[] arr = strDate.Split('-');
            string date = arr[0] + "/" + arr[1] + "/" + arr[2];
            return DateTime.ParseExact(date, "dd/MM/yyyy", null);
        }

        private Product setProductFromForm(Product product, FormCollection form, bool isNewProduct)
        {
            product.name = form["txtName"].Trim();
            product.specification_name = form["txtSpecificationName"].Trim();
            product.renovation_date = formatToDate(form["txtPurchaseDate"]);

            if (isNewProduct)
            {
                string expiration_date = product.renovation_date.ToString();

                if (form["chkLimitless"].Equals("false"))
                {
                    if (form["ddlTermType"].Equals("1"))
                    {
                        int addDays = Convert.ToInt32(form["txtNumber"]) * 365;
                        product.expiration_date = Convert.ToDateTime(expiration_date).AddDays(addDays);
                    }
                    else
                    {
                        int addDays = Convert.ToInt32(form["txtNumber"]) * 30;
                        product.expiration_date = Convert.ToDateTime(expiration_date).AddDays(addDays);
                    }
                }
                else
                {
                    product.expiration_date = Convert.ToDateTime("9999-12-12");
                }
            }
            else
            {
                product.id = Convert.ToInt32(form["id"]);
                product.expiration_date = form["chkLimitless"].Equals("false") ? formatToDate(form["txtExpirationDate"]) : Convert.ToDateTime("9999-12-12");
            }

            product.license_fee = form["txtLicenseFee"].Trim();
            product.currency_type = form["ddlCurrencyType"].Equals("1") ? "₺(TL)" : "$(USD)";
            product.license_type = form["txtLicenseType"].Trim();
            product.usage_type = form["ddlUsageType"].Equals("1");
            product.payer = form["txtPayer"].Trim();
            product.seller = form["txtSeller"].Trim();
            product.owner = form["txtOwner"].Trim();
            product.req_faculty = form["txtReqFaculty"].Trim();
            product.req_personnel = form["txtReqPersonnel"].Trim();
            product.users = form["txtUsers"].Trim();
            product.is_active = form["ddlIsActive"].Equals("1");
            product.explanation = form["txtExplanation"].Trim();

            return product;
        }
    }
}