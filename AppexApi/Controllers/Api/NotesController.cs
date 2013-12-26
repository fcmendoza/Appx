using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppexApi.Controllers.Api
{
    public class NotesController : ApiController
    {
        public IEnumerable<Note> Get()
        {
            return new List<Note> { 
                new Note { Url = "https://dl.dropboxusercontent.com/u/26506865/windows_clean_installation.txt" },
                new Note { Url = "https://dl.dropboxusercontent.com/u/26506865/links.txt" }
            };
        }
    }

    // TODO: maybe this shold be a REAL model that is shared by both the Api and the Web site/app.
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}