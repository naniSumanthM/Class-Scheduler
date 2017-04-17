using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace WSAD_FinalProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Login",
                url: "Login", //if the url contains this then append "name" instead
                defaults: new { controller = "Account", action="Login"}
            );

            routes.MapRoute(
                name: "Logout",
                url: "Logout", 
                defaults: new { controller = "Account", action = "Logout" }
            );

            //Does not work
            routes.MapRoute(
                name: "MyProfile",
                url: "UserProfile", 
                defaults: new { controller = "Account", action = "UserProfile", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
