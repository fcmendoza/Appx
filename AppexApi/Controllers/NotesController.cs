using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;
using System.Dynamic;

namespace AppexApi.Controllers
{
    public class NotesController : Controller
    {
        public ActionResult Index() {
            var dropbox = new Api.LinksController().GetDropBoxApiInstance();
            var files = dropbox.GetFiles(root: "dropbox", path: "Books");

            var thefiles = files.Contents
                .Where(x => x.Path.Contains(".txt"))
                .OrderByDescending(x => x.Modified)
                .Select(x => new NoteInfoVM {
                    Filename = x.Path.ToLower().Replace("/books/", String.Empty).Replace(".txt", String.Empty), 
                    ModifiedOn = x.Modified 
                });

            return View(thefiles);
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

        [System.Web.Mvc.Authorize]
        public ActionResult Booknotes(string filename, string style) {
            string directory = @"BookNotes";
            return DisplayTheContent(directory: directory, filename: filename, style: style);
        }

        private ActionResult DisplayTheContent(string directory, string filename, string style) {
            string text = new Api.LinksController().GetTextFromFile(directory: directory, filename: String.Format("{0}.txt", filename));

            ViewBag.Style = style;
            ViewBag.Title = filename;
            return View("Content", new MarkdownViewModel { Body = text });
        }
    }

    public class NoteInfoVM {
        public string Filename { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
