using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppexApi {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Custom route. Should be added before the Default one
            routes.MapRoute(
                name: "Notes",
                url: "notes/{filename}",
                defaults: new { controller = "Notes", action = "DisplayContent", style = UrlParameter.Optional }
            );

            // Custom route. Should be added before the Default one
            routes.MapRoute(
                name: "Todos",
                url: "todos",
                defaults: new { controller = "Notes", action = "DisplayContent", filename = "todos", style = UrlParameter.Optional }
            );

            // Custom route. Should be added before the Default one
            routes.MapRoute(
                name: "Books",
                url: "books",
                defaults: new { controller = "Notes", action = "DisplayContent", filename = "books", style = UrlParameter.Optional }
            );

            // Custom route. Should be added before the Default one
            routes.MapRoute(
                name: "Markdown",
                url: "markdown",
                defaults: new { controller = "Notes", action = "DisplayContent", filename="markdown-test", style = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}