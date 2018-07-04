using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Web;
using System.Web.Mvc;

namespace ToDo.Models
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(ToDo.userlogin userlogin, string user_password2)
        {
            if (!user_password2.Equals(userlogin.password))
            {
                ModelState.AddModelError("passwordVertify", "Password must be same.");
            }

            using (var dc = new todo_listEntities())
            {
                int countoflogin = dc.userlogins.Where(x => x.login.Equals(userlogin.login)).Count();
                if (countoflogin != 0)
                {
                    ModelState.AddModelError("login", "Login is already exist");
                }
            }

                if (ModelState.IsValid)
            {

                userlogin.password = EncryptHelper.encryptPassword(userlogin.password);
                using (var dc = new todo_listEntities())
                {
                    dc.userlogins.Add(userlogin);
                    dc.SaveChanges();
                }

            return RedirectToAction("Index", "Login");
            }
            else
            {
                return View("Index", userlogin);
            }
        }
    }
}