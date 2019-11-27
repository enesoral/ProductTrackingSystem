using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductTrackingSystem.Models;
using PagedList;

namespace ProductTrackingSystem.Controllers
{
    public class ProductController : Controller
    {
        private SoftwareTrackingEntities db = new SoftwareTrackingEntities();
        public ActionResult Index(int page = 1, int pageSize = 4)
        {
            if (pageSize > 20)
                pageSize = 20;

            List<Product> products = db.Products.SqlQuery("SELECT * FROM PRODUCTS WHERE active = 1").ToList();
            PagedList<Product> model = new PagedList<Product>(products, page, pageSize);

            return View(model);
        }
    }
}