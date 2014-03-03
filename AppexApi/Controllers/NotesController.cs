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
            var files = GetFileInfo(directory: "Books");
            return View(files);
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

            if (String.IsNullOrWhiteSpace(filename)) {
                var files = GetFileInfo(directory: directory);
                return View("Index", files);
            }
            else {
                return DisplayTheContent(directory: directory, filename: filename, style: style);
            }
        }

        private ActionResult DisplayTheContent(string directory, string filename, string style) {
            string text = _shared.GetTextFromFile(directory: directory, filename: String.Format("{0}.txt", filename));

            ViewBag.Style = style;
            ViewBag.Title = filename;
            ViewBag.Hightlight = null;

            if (Request.QueryString.AllKeys.Length > 0) {
                var keys = Request.QueryString.AllKeys.ToList();
                var exists = keys.Where(k => k.ToLower() == "highlight" || k.ToLower() == "syntax").Any();
                ViewBag.Hightlight = exists ? "highlight" : null;
            }
            
            return View("Content", new MarkdownViewModel { Body = text });
        }

        private IEnumerable<NoteInfoVM> GetFileInfo(string directory) {
            var dropbox = _shared.GetDropBoxApiInstance();
            var files = dropbox.GetFiles(root: "dropbox", path: directory);

            var thefiles = files.Contents
                .Where(x => x.Path.Contains(".txt"))
                .OrderByDescending(x => x.Modified)
                .Select(x => new NoteInfoVM {
                    Filename = x.Path.ToLower().Replace("/" + directory.ToLower() + "/", String.Empty).Replace(".txt", String.Empty),
                    ModifiedOn = TimeZoneHelper.UtcToPacific(x.Modified.UtcDateTime)
                });

            return thefiles;
        }

        private Shared _shared = new Shared();
    }

    public class NoteInfoVM {
        public string Filename { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
