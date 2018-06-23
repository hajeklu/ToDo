using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using ToDo.Models;

namespace ToDo.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Singin(String login, String password, String remember)
        {
            bool remem = remember != null;
            if (new ToDoMemberShipProvider().ValidateUser(login, EncryptHelper.encryptPassword(password)))
            {
                FormsAuthentication.SetAuthCookie(login, remem);
                return RedirectToAction("Index", "Home");
            }

            TempData["error"] = "Login or password is invalid";
            return RedirectToAction("Index", "Login");
        }
    }
}