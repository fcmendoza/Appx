using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppexApi.Models {
    public class Job {
        public string company { get; set; }
        public string title { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public string description { get; set; }
        public List<string> technologies { get; set; }
    }

    public class Link {
        public string name { get; set; }
        public string url { get; set; }
        public string displayName { get; set; }
        public string icon { get; set; }
        public int order { get; set; }
    }

    public class Resume {
        public string name { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public List<Job> jobs { get; set; }
        public string email { get; set; }
        public List<Link> links { get; set; }
    }
}