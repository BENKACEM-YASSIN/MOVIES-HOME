using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoviesHome.Models;

namespace MoviesHome.Areas.Admin.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        Context _dbContext = new Context();
        public ActionResult Index()
        {
            int pageSize = 10;
            int pageId = Convert.ToInt32(Request.QueryString["pageId"]);
            List<Movie> movies = _dbContext.Movies.ToList();
            double pageCount = (double)movies.Count / (double)pageSize;
            ViewBag.PageCount = Math.Ceiling(pageCount);

            movies = movies.Skip((pageId - 1) * pageSize).Take(pageSize).ToList();

            return View(movies);
        }

        public ActionResult Add(int? id)
        {
            if(id == null)
                return View();

            Movie movie = _dbContext.Movies.Find(id);
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult Add(Movie movie)
        {
            if (ModelState.IsValid)
            {
                if(movie.ImageFile != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(movie.ImageFile.FileName);
                    string FileExtension = Path.GetExtension(movie.ImageFile.FileName);
                    FileName = FileName.Trim() + FileExtension;
                    string appPath = Request.PhysicalApplicationPath;
                    string UploadPath = appPath + "/Content/images/";
                    movie.Image = FileName;
                    movie.ImageFile.SaveAs(UploadPath + FileName);
                }

                if(movie.Id != 0)
                {
                    Movie movieUpdated = _dbContext.Movies.Find(movie.Id);
                    movieUpdated.Title = movie.Title;
                    movieUpdated.Description = movie.Description;
                    movieUpdated.Duration = movie.Duration;
                    movieUpdated.ReleaseDate = movie.ReleaseDate;
                    movieUpdated.Country = movie.Country;
                    movieUpdated.Rating = movie.Rating;
                    if (movie.Image != null)
                        movieUpdated.Image = movie.Image;
                } else
                {
                    _dbContext.Movies.Add(movie);
                }

                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            if(id != 0)
            {
                Movie movie = _dbContext.Movies.Find(id);
                _dbContext.Movies.Remove(movie);
                _dbContext.SaveChanges();
            }

            List<Movie> movies = _dbContext.Movies.ToList();
            return RedirectToAction("Index");
        }
    }
}