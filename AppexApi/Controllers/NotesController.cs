using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;

namespace AppexApi.Controllers
{
    public class NotesController : Controller
    {
        public ActionResult Index() {
            return null;
        }

        public ActionResult DisplayContent(string filename, string password, string style) {

            var restrictedFiles = ConfigurationManager.AppSettings["RestrictedFiles"].ToLower().Split(',').ToList().Select(f => f.Trim());

            bool authorized = restrictedFiles.Contains(filename.Trim()) ? _auth.ValidatePassword(password) : true;

            if (authorized) {
                string text = new Api.LinksController().GetTextFromFile(String.Format("{0}.txt", filename));

                ViewBag.Style = style;
                ViewBag.Title = filename;
                return View("Content", new MarkdownViewModel { Body = text });
            }
            else {
                return View("../Shared/Unauthorized");
            }
        }

        private AuthenticationController _auth = new AuthenticationController();
    }
}
