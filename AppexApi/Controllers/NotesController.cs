using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace AppexApi.Controllers
{
    public class NotesController : Controller
    {
        public ActionResult Index() {
            return null;
        }

        public ActionResult DisplayContent(string filename, string style) {
            string text = new Api.LinksController().GetTextFromFile(String.Format("{0}.txt", filename));

            ViewBag.Style = style;
            ViewBag.Title = filename;
            return View("Content", new MarkdownViewModel { Body = text });
        }
    }
}
