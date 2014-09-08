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
                    string text = _shared.GetTextFromFile(String.Format("{0}.txt", filename));

                    ViewBag.Style = style;
                    ViewBag.Title = filename;
                    return View("../Notes/Content", new MarkdownViewModel { Body = text });
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Login() {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string password, string returnUrl) {
            bool authorized = _auth.ValidatePassword(password);

            if (authorized) {
                System.Web.Security.FormsAuthentication.SetAuthCookie(userName: "guest", createPersistentCookie: false);
                return Redirect(returnUrl);
            }
            else {
                ViewBag.ErrorMessage = "Invalid credentials. Please try again.";
            }

            return View();
        }

        private AuthenticationController _auth = new AuthenticationController();
        private Shared _shared = new Shared(new DropboxContentRepository());
    }
}
