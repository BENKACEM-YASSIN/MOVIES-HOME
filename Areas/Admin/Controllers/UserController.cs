using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoviesHome.Models;

namespace MoviesHome.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        Context _dbContext = new Context();
        public ActionResult Index()
        {
            int pageSize = 10;
            int pageId = Convert.ToInt32(Request.QueryString["pageId"]);
            List<User> users = _dbContext.Users.ToList();
            double pageCount = (double)users.Count / (double)pageSize;
            ViewBag.PageCount = Math.Ceiling(pageCount);

            users = users.Skip((pageId - 1) * pageSize).Take(pageSize).ToList();

            return View(users);
        }

        public ActionResult Add(int? id)
        {
            if (id == null)
                return View();

            User user = _dbContext.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult Add(User user)
        {
            if (ModelState.IsValid)
            { }

            return View(user);
        }

        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                User user = _dbContext.Users.Find(id);
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}