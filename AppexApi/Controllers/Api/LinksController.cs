using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OAuthProtocol;
using Dropbox.Api;

namespace AppexApi.Controllers.Api
{
    public class LinksController : ApiController
    {
        public LinksController() {
            _consumerKey    = System.Configuration.ConfigurationManager.AppSettings["AppKey"];
            _consumerSecret = System.Configuration.ConfigurationManager.AppSettings["AppSecret"];
            _oauthToken     = System.Configuration.ConfigurationManager.AppSettings["OAuthToken"];
        }

        public IEnumerable<Link> Get() {
            return GetLinks();
        }

        public Link Get(int id) {
            var link = GetLink(id);
            if (link == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else {
                return link;
            }
        }

        public IEnumerable<Link> Get(string title) {
            return GetLinks().Where(l => l.Title.ToLower().Contains(title.ToLower()));
        }

        public HttpResponseMessage PostLink(Link link) {
            int id = Create(link);
            var response = Request.CreateResponse<Link>(HttpStatusCode.Created, link);
            string uri = Url.Link("DefaultApi", new { id = id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public HttpResponseMessage PutLink(int id, Link link) {
            link.Id = id;
            if (!Update(link)) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else { 
                return Request.CreateResponse(HttpStatusCode.OK); 
            }
        }

        public HttpResponseMessage DeleteLink(int id) {
            if (!Delete(id)) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        public string GetHttmlFromMarkdown(string url) {
            var client = new WebClient();
            var html = client.DownloadString(url);
            return html;
        }

        public string GetTextFromFile(string filename, string directory = "Books") {
            var accessToken = new OAuthToken(token: _oauthToken.Substring(0, 16), secret: _oauthToken.Substring(18, 15));
            var api = new DropboxApi(_consumerKey, _consumerSecret, accessToken);
            var file = api.DownloadFile("dropbox", String.Format("{0}/{1}", directory, filename));
            return file.Text;
        }

        public DropboxApi GetDropBoxApiInstance() {
            var accessToken = new OAuthToken(token: _oauthToken.Substring(0, 16), secret: _oauthToken.Substring(18, 15));
            return new DropboxApi(_consumerKey, _consumerSecret, accessToken);
        }

        private string _consumerKey;
        private string _consumerSecret;
        private string _oauthToken;

        /*
            TODO: this private methods must be in a repository that connects to the data source.
        */

        private IEnumerable<Link> GetLinks() {
            return _links;
        }

        private Link GetLink(int id) {
            return _links.Where(l => l.Id == id).FirstOrDefault();
        }

        private int Create(Link link) {
            int id = _links.Max(x => x.Id) + 1;
            link.Id = id;

            var links = _links.ToList();
            links.Add(link);
            return link.Id;
        }

        private bool Update(Link link) {
            int index = _links.ToList().FindIndex(i => i.Id == link.Id);

            if (index == -1) {
                return false; // link does not exist.
            }
            else {
                // here goes the logic that updates the link in the datasource.
                return true;
            }
        }

        private bool Delete(int id) {
            int index = _links.ToList().FindIndex(i => i.Id == id);

            if (index == -1) {
                return false; // link does not exist.
            }
            else {
                var links = _links.ToList();
                links.RemoveAll(l => l.Id == id);
                return true;
            }
        }

        private IEnumerable<Link> _links = new Link[] { 
            new Link { Id = 1, Title ="Google", Url = "http://google.com", CreatedOn = DateTime.UtcNow, Description = "Lorem ipsum dolor sit amet." }, 
            new Link { Id = 2, Title ="Microsoft", Url = "http://microsoft.com", CreatedOn = DateTime.UtcNow, Description = "Lorem ipsum dolor sit amet." },
            new Link { Id = 3, Title ="Apple", Url = "http://apple.com", CreatedOn = DateTime.UtcNow, Description = "Lorem ipsum dolor sit amet." },
            new Link { Id = 4, Title ="Google's Geolocation API", Url = "https://developers.google.com/maps/documentation/geocoding", CreatedOn = DateTime.UtcNow, Description = "Google's Geolocation API." },
            new Link { Id = 5, Title ="Azure Deployment With Git", Url = "http://blogs.msdn.com/b/carlosfigueira/archive/2012/07/12/creating-asp-net-web-apis-on-azure-web-sites.aspx", CreatedOn = DateTime.UtcNow, Description = "How to deploy to Azure websites using git." },
            new Link { Id = 6, Title =".NET Fiddle", Url = "http://dotnetfiddle.net", CreatedOn = DateTime.UtcNow, Description = "Like js fiddle but for .NET scripts." },
            new Link { Id = 7, Title ="Clean Install OS X Mavericks", Url = "http://osxdaily.com/2013/10/26/clean-install-os-x-mavericks", CreatedOn = DateTime.UtcNow, Description = "How to Clean Install OS X Mavericks." },
            new Link { Id = 8, Title ="Serialize Json camelCasing", Url = "http://www.asp.net/web-api/overview/formats-and-model-binding/json-and-xml-serialization#json_camelcasing", CreatedOn = DateTime.UtcNow, Description = "How to serialize JSON as camel casing in ASP.NET MVC 4 Web Api." },
            new Link { Id = 9, Title ="Web API CRUD operations", Url = "http://www.asp.net/web-api/overview/creating-web-apis/creating-a-web-api-that-supports-crud-operations", CreatedOn = DateTime.UtcNow, Description = "Creating a Web API (ASP.NET MVC 4) that supports CRUD operations. Sample tutorial." },
            new Link { Id = 10, Title ="Lenovo ThinkPad X1 Carbon Touch", Url = "http://www.amazon.com/gp/product/B00AQ2DS8S/", CreatedOn = DateTime.UtcNow, Description = "Lenovo ThinkPad X1 Carbon 14-Inch Touchscreen Laptop (Black)3444CUU. $1,600.00 USD." },
            new Link { Id = 11, Title ="Track Fedex", Url = "https://www.fedex.com/fedextrack/index.html?tracknumbers=588151315053533&cntry_code=us", CreatedOn = DateTime.UtcNow, Description = "Track Fedex package (Lenovo laptop)." },
            new Link { Id = 12, Title ="Markdown ASP.NET", Url = "http://stackoverflow.com/questions/5320922/how-to-use-asp-net-mvc-3-and-stackoverflows-markdown", CreatedOn = DateTime.UtcNow, Description = "StackOverflow question (and accepted answer) about how to integrate markdown in ASP.NET MVC." },
        };
    }

    // TODO: maybe this shold be a REAL model that is shared by both the Api and the Web site/app.
    public class Link {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}


/*
    GET http://localhost:64250/api/links/2 HTTP/1.1
    Content-Type: application/json
 * 
 * 
    GET http://localhost:64250/api/links?title=pp HTTP/1.1
    Content-Type: application/json
 * 
 * 
    POST http://localhost:64250/api/links HTTP/1.1
    Content-Type: application/json

    {"title":"Amazon","url":"http://amazon.com","description":"Lorem ipsum dolor sit amet."}
 * 
 * 
    PUT http://localhost:64250/api/links/1 HTTP/1.1
    Content-Type: application/json

    {"title":"Google","url":"http://google.com","description":"Lorem ipsum dolor sit amet."}
 * 
 * 
    DELETE http://localhost:64250/api/links/1 HTTP/1.1
    Content-Type: application/json

*/
