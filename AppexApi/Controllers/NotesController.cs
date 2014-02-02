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

        public ActionResult DisplayContent(string filename, string style) {
            string directory = "Books";
            return DisplayTheContent(directory: directory, filename: filename, style: style);
        }

        [System.Web.Mvc.Authorize]
        public ActionResult Private(string filename, string style) {
            string directory = @"Books/private";
            return DisplayTheContent(directory: directory, filename: filename, style: style);
        }

        private ActionResult DisplayTheContent(string directory, string filename, string style) {
            string text = new Api.LinksController().GetTextFromFile(directory: directory, filename: String.Format("{0}.txt", filename));

            ViewBag.Style = style;
            ViewBag.Title = filename;
            return View("Content", new MarkdownViewModel { Body = text });
        }
    }
}
