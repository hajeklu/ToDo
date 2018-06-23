using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
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
                itemsList = dc.items.ToList();
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
        public ActionResult ajaxCheck(DataModel model)
        {
            List<item> items = null;
            using (var dc = new todo_listEntities())
            {
                item updateItem = dc.items.Where(i => i.iditem == model.iditem).FirstOrDefault();
                updateItem.done = Convert.ToSByte(model.value);
                dc.SaveChanges();

                items = dc.items.ToList();
            }
            return new JsonResult {Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }
    }
}