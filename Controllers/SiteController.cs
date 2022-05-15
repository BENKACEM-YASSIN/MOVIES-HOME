using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MoviesHome.Models;

namespace MoviesHome.Controllers
{
    public class SiteController : Controller
    {
        Context _dbContext = new Context();

        public ActionResult Index()
        {
            return View(PopulateData(""));
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult Index(string sortValue)
        {
            sortValue = Request.Form["drpSort"];
            ViewBag.SortValue = sortValue;
            return View(PopulateData(sortValue));
        }

        public List<Movie> PopulateData(string sortValue)
        {
            int pageSize = 5;
            int pageId = Convert.ToInt32(Request.QueryString["pageId"]);
            List<Movie> movies = _dbContext.Movies.ToList();

            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList.Add(new SelectListItem { Text = "By Title", Value = "Title" });
            drpList.Add(new SelectListItem { Text = "By Release Date", Value = "ReleaseDate" });
            drpList.Add(new SelectListItem { Text = "By Rating Ascending", Value = "Rating" });
            drpList.Add(new SelectListItem { Text = "By Rating Descending", Value = "Rating DESC" });

            if (!string.IsNullOrEmpty(sortValue))
            {
                var value = drpList.Where(c => c.Value == ViewBag.SortValue).FirstOrDefault();
                value.Selected = true;
            }

            movies = FilterData(movies);

            ViewBag.SortValues = drpList;

            switch (sortValue)
            {
                case "Title":
                    movies = movies.OrderBy(x => x.Title).ToList();
                    break;
                case "ReleaseDate":
                    movies = movies.OrderBy(x => x.ReleaseDate).ToList();
                    break;
                case "Rating":
                    movies = movies.OrderBy(x => x.Rating).ToList();
                    break;
                case "Rating DESC":
                    movies = movies.OrderByDescending(x => x.Rating).ToList();
                    break;
            }

            double pageCount = (double)movies.Count / (double)pageSize;
            ViewBag.PageCount = Math.Ceiling(pageCount);

            movies = movies.Skip((pageId - 1) * pageSize).Take(pageSize).ToList();

            return movies;
        }

        public List<Movie> FilterData(List<Movie> movies)
        {
            string keyword = Request.Form["txtKeyword"];
            int fromYear = 0;
            int.TryParse(Request.Form["drpFromYear"], out fromYear);
            int toYear = 0;
            int.TryParse(Request.Form["drpToYear"], out toYear);
            double ratingFrom = 0;
            double.TryParse(Request.Form["txtRatingFrom"], out ratingFrom);
            double ratingTo = 0;
            double.TryParse(Request.Form["txtRatingTo"], out ratingTo);

            if (!string.IsNullOrEmpty(keyword))
            {
                movies = movies.Where(x => x.Title.ToLower().Contains(keyword.ToLower()) || x.Description.ToLower().Contains(keyword.ToLower())).ToList();
            }

            if (fromYear != 0)
            {
                movies = movies.Where(x => x.ReleaseDate.Year >= fromYear).ToList();
            }

            if (toYear != 0)
            {
                movies = movies.Where(x => x.ReleaseDate.Year <= toYear).ToList();
            }

            if (ratingFrom != 0)
            {
                movies = movies.Where(x => x.Rating >= ratingFrom).ToList();
            }

            if (ratingTo != 0)
            {
                movies = movies.Where(x => x.Rating <= ratingTo).ToList();
            }

            return movies;
        }

        public ActionResult Details(int id)
        {
            Movie movie = _dbContext.Movies.Find(id);

            return View(movie);
        }


        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Movie", new {area = "Admin"});
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                bool IsValidUser = _dbContext.Users
                .Any(u => u.Username.ToLower() == user.Username.ToLower()
                 && u.Password == user.Password);

                if (IsValidUser)
                {
                    FormsAuthentication.RedirectFromLoginPage(user.Username, false);
                }
            }

            ModelState.AddModelError("", "Invalid Username or Password");

            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}