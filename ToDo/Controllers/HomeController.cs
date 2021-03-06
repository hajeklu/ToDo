﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using Microsoft.Ajax.Utilities;
using ToDo.Models;
using WebGrease.Css.Ast.Selectors;
namespace ToDo.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    [Authorize]
    public class HomeController : Controller
    {
        // main page
        [Authorize]
        public ActionResult Index()
        {
            List<item> itemsList = null;
            using (var dc = new todo_listEntities())
            {
                ViewBag.name = User.Identity.Name;
                int id = getUserId();
                itemsList = dc.items.ToList().Where(m => m.userlogin.iduser == id).ToList();
                List<String> agoTimes = new List<string>();
                foreach (item item in itemsList)
                {
                    agoTimes.Add(TimeAgo(item.creationTime));
                }


                ViewBag.agotime = agoTimes;

            };
            return View(itemsList);
        }

        // add new item 
        [HttpPost]
        [Authorize]
        public ActionResult Add(String description)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }

            var i = new item();
            i.description = description;
            i.done = 0;
            i.id_userlogin = getUserId();
            i.creationTime = DateTime.Now;
            using (var dc = new todo_listEntities())
            { 
                dc.items.Add(i);
                dc.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Delete(item item)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }

            using (var dc = new todo_listEntities())
            {
                var i = dc.items.SingleOrDefault(it => it.iditem == item.iditem);
                dc.items.Remove(i);
                dc.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        public String ajaxCheck(DataModel model)
        {

            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index", "Login");
            }

            List<item> items = null;
            using (var dc = new todo_listEntities())
            {
                item updateItem = dc.items.Where(i => i.iditem == model.iditem).FirstOrDefault();
                updateItem.done = Convert.ToSByte(model.value);
                dc.SaveChanges();

                items = dc.items.Include(m => m.userlogin).ToList();
            }
            // return new JsonResult {Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
            return "{\"msg\":\"success\"}";
        }

        public void Logout()
        { 
            Response.Cookies.Clear();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetNoStore();
            var cookie = Request.Cookies["myCookie"];
            FormsAuthentication.SignOut();
            cookie.Values["id"] = null;
            Response.Redirect(Url.Action("Index", "Login"));
            
        }

        private int getUserId()
        {
            var cookie = Request.Cookies["myCookie"];
            string var = cookie.Values["id"];
            return Convert.ToInt32(cookie.Values["id"]);
        }

        private string TimeAgo(DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("about {0} minutes ago", timeSpan.Minutes) :
                    "about a minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("about {0} hours ago", timeSpan.Hours) :
                    "about an hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("about {0} days ago", timeSpan.Days) :
                    "yesterday";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("about {0} months ago", timeSpan.Days / 30) :
                    "about a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("about {0} years ago", timeSpan.Days / 365) :
                    "about a year ago";
            }

            return result;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            //   base.OnException(filterContext);
            filterContext.ExceptionHandled = true;

            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/ErrorHandler/Index.cshtml"
            };
        }
    }
}