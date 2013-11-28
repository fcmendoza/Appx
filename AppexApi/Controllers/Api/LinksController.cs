using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppexApi.Controllers.Api
{
    public class LinksController : ApiController
    {
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
