using System.Web.Mvc;
using System.Web.Routing;

namespace AutoCare.Product.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //catch all route from MVC and redirect to home/index
            //from here, angular will activate it's route

            //routes.MapRoute(
            //    name: "Account",
            //    url: "Account/{action}/{ReturnUrl}",
            //    defaults: new { controller = "Home", action = "Index", ReturnUrl = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "File",
                url: "File/{action}/{id}",
                defaults: new { controller = "File", action = "Get", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Account",
                url: "Account/{action}/{id}",
                defaults: new { controller = "Account", action = "LogOff", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "default",
                url: "Home/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DefaultOverride",
                url: "{*.}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
