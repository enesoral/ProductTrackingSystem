using ProductTrackingSystem.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductTrackingSystem.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index(int? errorId)
        {
            if (errorId < 0)
            {
                TempData["message"] = "Fail";
            }
            return View();
        }


        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            User user = new User
            {
                email = form["email"],
                password = form["password"]

            };

            if(IsValid(user))
            {
                return RedirectToAction("Index", "Product");
            }
            else
            {
                return RedirectToAction("Index", new { errorId = -1 });
            }
            
        }

        private bool IsValid(User user)
        {
            using (var db = new ProductTrackingEntities())
            {
                User validUser = db.Users.Where(x => x.email == user.email && x.password == user.password).FirstOrDefault();

                return validUser != null;
            }
        }
    }
}