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
            return new Link[] { 
                new Link { Title ="Google", Url = "http://google.com" }, 
                new Link { Title ="Microsoft", Url = "http://microsoft.com" }, 
                new Link { Title ="Apple", Url = "http://apple.com" } 
            };
        }
    }

    // TODO: maybe this shold be a REAL model that is shared by both the Api and the Web site/app.
    public class Link {
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
