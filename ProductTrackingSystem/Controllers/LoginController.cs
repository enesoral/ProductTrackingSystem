using ProductTrackingSystem.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProductTrackingSystem.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
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
                FormsAuthentication.SetAuthCookie(user.email, false);
                return RedirectToAction("Index", "Product");
            }
            else
            {
                TempData["message"] = "Fail";
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
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