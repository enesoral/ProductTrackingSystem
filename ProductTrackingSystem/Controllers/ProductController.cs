using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductTrackingSystem.Models;
using PagedList;
using ProductTrackingSystem.Models.Entity;

namespace ProductTrackingSystem.Controllers
{
    public class ProductController : Controller
    {
        private ProductTrackingEntities db = new ProductTrackingEntities();
        public ActionResult Index(string searching, int page = 1, int pageSize = 4)
        {
            if (pageSize > 20)
                pageSize = 20;

            var products = db.Products.Where(p => p.is_active);

            if (!String.IsNullOrEmpty(searching))
                if (!searching.Trim().Equals(""))
                    products = products.Where(x => x.name.Contains(searching.Trim()) || x.specification_name.Contains(searching.Trim()));



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