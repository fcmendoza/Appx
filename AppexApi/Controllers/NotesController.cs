using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppexApi.Controllers
{
    public class NotesController : Controller
    {
        public ActionResult Index() {
            return null;
        }

        public ActionResult DisplayContent(string filename, string style) {
            string htmlText = new Api.LinksController().GetHttmlFromMarkdown(url: String.Format("https://dl.dropboxusercontent.com/u/26506865/{0}.txt", filename));

            ViewBag.Style = style;
            ViewBag.Title = filename;
            return View("Content", new MarkdownViewModel { Body = htmlText });
        }
    }
}
