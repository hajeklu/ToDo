using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using Microsoft.Ajax.Utilities;
using ToDo.Models;

namespace ToDo.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            return View();
        }

        [HttpPost]
        public ActionResult Singin(String login, String password, String remember)
        {
            bool remem = remember != null;
            if (new ToDoMemberShipProvider().ValidateUser(login, EncryptHelper.encryptPassword(password)))
            {
                using (var dc = new todo_listEntities())
                {
                    userlogin user = dc.userlogins.FirstOrDefault(u => u.login.Equals(login));
                    var myCookie = new HttpCookie("myCookie");
                    myCookie.Values.Add("id", user.iduser.ToString());
                    Response.Cookies.Add(myCookie);
                    FormsAuthentication.SetAuthCookie(login, remem);
                }
                return RedirectToAction("Index", "Home");
            }

            TempData["error"] = "Login or password is invalid";
            return RedirectToAction("Index", "Login");
        }
    }
}