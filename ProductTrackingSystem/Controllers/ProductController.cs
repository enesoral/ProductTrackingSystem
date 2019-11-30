using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using ProductTrackingSystem.Models;
using PagedList;
using ProductTrackingSystem.Models.Entity;

namespace ProductTrackingSystem.Controllers
{
    public class ProductController : Controller
    {
        private ProductTrackingEntities db = new ProductTrackingEntities();
        public ActionResult Index(string searching, int ddlListType = 1, int ddlMonth = 1, int page = 1, int pageSize = 4)
        {

            if (pageSize > 20)
                pageSize = 20;

            List<Product> products = null;

            switch (ddlListType)
            {
                case 2:
                    products = new List<Product>(db.Products.Where(p => !p.is_active));
                    break;
                case 3:
                    products = new List<Product>(db.Products.SqlQuery(
                        "SELECT * FROM Products WHERE DATEDIFF(day, GETDATE(), expiration_date) <= {0} AND DATEDIFF(day, GETDATE(), expiration_date) >= 0", ddlMonth * 30));
                    break;
                case 4:
                    products = new List<Product>(db.Products.SqlQuery(
                        "SELECT * FROM Products WHERE DATEDIFF(day, GETDATE(), expiration_date) <= 0"));
                    break;
                case 5:
                    products = new List<Product>(db.Products.SqlQuery(
                        "SELECT * FROM Products WHERE MONTH(renovation_date) <= 6 AND YEAR(renovation_date) = YEAR(GETDATE())"));
                    break;
                case 6:
                    products = new List<Product>(db.Products.SqlQuery(
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

            PagedList<Product> model = new PagedList<Product>(products.ToList(), page, pageSize);

            return View(model);
        }

        public ActionResult Detail(int id)
        {
            var model = db.Products.ToList().FirstOrDefault(x => x.id == id);
            return PartialView("Detail", model);
        }
    }
}