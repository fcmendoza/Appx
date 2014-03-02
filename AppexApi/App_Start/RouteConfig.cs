using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppexApi {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Custom routes should be added before the Default one.

            routes.MapRoute(
                 name: "BooknotesIndex",
                 url: "booknotes/",
                 defaults: new { controller = "Notes", action = "Booknotes", style = UrlParameter.Optional }
            );

            routes.MapRoute(
                 name: "Booknotes",
                 url: "booknotes/{filename}",
                 defaults: new { controller = "Notes", action = "Booknotes", style = UrlParameter.Optional }
            );

            routes.MapRoute(
                 name: "PrivateNotes",
                 url: "notes/private/{filename}",
                 defaults: new { controller = "Notes", action = "Private", style = UrlParameter.Optional }
            );

             routes.MapRoute(
                 name: "Notes",
                 url: "notes/{filename}",
                 defaults: new { controller = "Notes", action = "DisplayContent", style = UrlParameter.Optional }
             );

            routes.MapRoute(
                name: "Moni",
                url: "monica",
                defaults: new { controller = "Notes", action = "DisplayContent", filename = "monica", style = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Todos",
                url: "todos",
                defaults: new { controller = "Notes", action = "DisplayContent", filename = "todos", style = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Books",
                url: "books",
                defaults: new { controller = "Notes", action = "DisplayContent", filename = "books", style = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Markdown",
                url: "markdown",
                defaults: new { controller = "Notes", action = "DisplayContent", filename="markdown-test", style = UrlParameter.Optional }
            );

            // Default route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}