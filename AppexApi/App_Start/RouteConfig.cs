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

            bool redirectionEnabled = bool.TryParse(ConfigurationManager.AppSettings["CustomUrlRedirectionEnabled"], out redirectionEnabled) ? redirectionEnabled : false;

            if (redirectionEnabled) {
                routes.Add(new SubdomainRoute());
            }

            // Custom route. Should be added before the Default one
            routes.MapRoute(
                name: "Moni",
                url: "monica",
                defaults: new { controller = "Notes", action = "DisplayContent", filename = "monica", style = UrlParameter.Optional }
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

    public class SubdomainRoute : RouteBase {

        public override RouteData GetRouteData(HttpContextBase httpContext) {
            //var url = httpContext.Request.Headers["HOST"];
            //var index = url.IndexOf(".");

            //if (index < 0)
            //    return null;

            //var subDomain = url.Substring(0, index);

            //if (subDomain == "user1") {
            //    var routeData = new RouteData(this, new MvcRouteHandler());
            //    routeData.Values.Add("controller", "User1"); //Goes to the User1Controller class
            //    routeData.Values.Add("action", "Index"); //Goes to the Index action on the User1Controller

            //    return routeData;
            //}

            //if (subDomain == "user2") {
            //    var routeData = new RouteData(this, new MvcRouteHandler());
            //    routeData.Values.Add("controller", "User2"); //Goes to the User2Controller class
            //    routeData.Values.Add("action", "Index"); //Goes to the Index action on the User2Controller

            //    return routeData;
            //}

            string hostname = httpContext.Request.Url.Host;
            string value = ConfigurationManager.AppSettings[hostname] ?? null;

            if (value != null) {
                var routeData = new RouteData(this, new MvcRouteHandler());
                routeData.Values.Add("controller", "Notes"); //Goes to the NotesController class
                routeData.Values.Add("action", "DisplayContent"); //Goes to the DisplayContent action on the NotesController
                routeData.Values.Add("filename", value); // sets the filename parameter to the specified in the web.config

                return routeData;
            }

            return null;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            //Implement your formating Url formating here
            return null;
        }
    }
}