using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using ToDo.Models;
using WebGrease.Css.Ast.Selectors;
namespace ToDo.Controllers
{
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
                int id = getUserId();
                itemsList = dc.items.ToList().Where(m => m.userlogin.iduser == id).ToList();
            };
            return View(itemsList);
        }

        // add new item 
        [HttpPost]
        public ActionResult Add(String description)
        {

            var i = new item();
            i.description = description;
            i.done = 0;
            i.id_userlogin = getUserId();
            using (var dc = new todo_listEntities())
            { 
                dc.items.Add(i);
                dc.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public ActionResult Delete(item item)
        {
            using (var dc = new todo_listEntities())
            {
                var i = dc.items.SingleOrDefault(it => it.iditem == item.iditem);
                dc.items.Remove(i);
                dc.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public String ajaxCheck(DataModel model)
        {
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

        private int getUserId()
        {
            var cookie = Request.Cookies["myCookie"];
            string var = cookie.Values["id"];
            return Convert.ToInt32(cookie.Values["id"]);
        }
    }
}