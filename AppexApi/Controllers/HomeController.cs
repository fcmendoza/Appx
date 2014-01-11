using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppexApi.Controllers {
    public class HomeController : Controller {
        public ActionResult Index(string style) {
            bool redirectionEnabled = bool.TryParse(ConfigurationManager.AppSettings["CustomUrlRedirectionEnabled"], out redirectionEnabled) ? redirectionEnabled : false;
            
            if (redirectionEnabled) {
                string hostname = HttpContext.Request.Url.Host;
                string filename = ConfigurationManager.AppSettings[hostname] ?? null;

                if (filename != null) {
                    string text = new Api.LinksController().GetTextFromFile(String.Format("{0}.txt", filename));

                    ViewBag.Style = style;
                    ViewBag.Title = filename;
                    return View("../Notes/Content", new MarkdownViewModel { Body = text });
                }
            }

            return View();
        }
    }
}
