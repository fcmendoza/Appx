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
        public ActionResult Index(bool? isLocal) {
            var files = GetFileInfo(directory: isLocal == true ? null : "Books");
            return View(files);
        }

        public ActionResult DisplayContent(string filename, string style) {
            string directory = "Books";
            return DisplayTheContent(directory: directory, filename: filename, style: style);
        }

        public ActionResult DisplayLocalContent(string filename, string style) {
            string directory = null; 
            return DisplayTheContent(directory: directory, filename: filename, style: style);
        }

        [System.Web.Mvc.Authorize]
        public ActionResult Private(string filename, string style) {
            string directory = @"Books/private";
            return DisplayTheContent(directory: directory, filename: filename, style: style);
        }

        [System.Web.Mvc.Authorize]
        public ActionResult Booknotes(string filename, string style, string directory = "BookNotes") {
            if (String.IsNullOrWhiteSpace(filename)) {
                var files = GetFileInfo(directory: directory);
                return View("Index", files);
            }
            else {
                return DisplayTheContent(directory: directory, filename: filename, style: style);
            }
        }

        private ActionResult DisplayTheContent(string directory, string filename, string style) {
            ViewBag.Style = style;
            ViewBag.Title = filename;
            ViewBag.Hightlight = null;

            bool nocache = false;

            if (Request.QueryString.AllKeys.Length > 0) {
                var keys = Request.QueryString.AllKeys.ToList();
                var exists = keys.Where(k => k.ToLower() == "highlight" || k.ToLower() == "syntax").Any();
                ViewBag.Hightlight = exists ? "highlight" : null;

                nocache = keys.Where(k => k.ToLower() == "nocache" || k.ToLower() == "no-cache").Any();
            }

            if (directory == null) {
                _shared = new Shared(new LocalContentRepository(_localPath));
            }
            else {
                _shared = new Shared(new DropboxContentRepository());
            }

            string text = _shared.GetTextFromFile(directory: directory, filename: String.Format("{0}.txt", filename), cacheTimeoutInSeconds: nocache ? 0 : -1);

            return View("Content", new MarkdownViewModel { Body = text });
        }

        private IEnumerable<NoteInfoVM> GetFileInfo(string directory) {
            if (directory == null) {
                _shared = new Shared(new LocalContentRepository(_localPath));
            }
            else {
                _shared = new Shared(new DropboxContentRepository());
            }

            var files = _shared.GetFiles(directory);

            var thefiles = files
                .Where(x => x.Path.Contains(".txt"))
                .OrderByDescending(x => x.Modified)
                .Select(x => new NoteInfoVM {
                    Filename = x.Path.ToLower().Replace("/" + (directory != null ? directory.ToLower() : String.Empty) + "/", String.Empty).Replace(".txt", String.Empty),
                    ModifiedOn = TimeZoneHelper.UtcToPacific(x.Modified.UtcDateTime)
                });

            return thefiles;
        }

        private string _localPath = ConfigurationManager.AppSettings["LocalNotesDirectory"];
        private Shared _shared;
    }

    public class NoteInfoVM {
        public string Filename { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
