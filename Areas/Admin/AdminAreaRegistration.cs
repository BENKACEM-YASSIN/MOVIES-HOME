using System.Web.Mvc;

namespace MoviesHome.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin",
                "admin/{controller}/{action}/{id}",
                new { controller = "Movie", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}