using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kiwi.Markdown;
using MarkdownSharp;

namespace AppexApi.Controllers
{
    public class LinksController : Controller
    {
        public ActionResult Index() {
            return View();
        }
    }

    public class MarkdownViewModel {
        public string Body { get; set; }
    }

    public static class MarkdownHelper {
        /// <summary>
        /// An instance of the Markdown class that performs the transformations.
        /// </summary>
        static Markdown markdownTransformer = new Markdown();

        /// <summary>
        /// Transforms a string of Markdown into HTML.
        /// </summary>
        /// <param name="helper">HtmlHelper - Not used, but required to make this an extension method.</param>
        /// <param name="text">The Markdown that should be transformed.</param>
        /// <returns>The HTML representation of the supplied Markdown.</returns>
        public static IHtmlString Markdown(this HtmlHelper helper, string text) {
            // Transform the supplied text (Markdown) into HTML.
            string html = markdownTransformer.Transform(text);

            // This 'replaces' were added by me (fer)
            html = html.Replace("<p><code>", "<pre><code>");
            html = html.Replace("</code></p>", "</code></pre>");

            // Wrap the html in an MvcHtmlString otherwise it'll be HtmlEncoded and displayed to the user as HTML :(
            return new MvcHtmlString(html);
        }

        public static IHtmlString MarkdownColorized(this HtmlHelper helper, string text) {
            string html = KiwiMarkdownService.Instance.ToHtml(text);
            return new MvcHtmlString(html);
        }
    }

    public class KiwiMarkdownService {
        private static readonly Lazy<IMarkdownService> Singleton;

        public static IMarkdownService Instance {
            get { return Singleton.Value; }
        }

        static KiwiMarkdownService() {
            Singleton = new Lazy<IMarkdownService>(
                () => new MarkdownService(), true);
        }
    }
}
