using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace herman.Models
{
    public class tmdbSearchResult
    {
        public string name { get; set; }

        public string tmdbID { get; set; }

        public string profile_path { get; set; }

        public string file_path { get; set; }

        string width { get; set; }

        public string id { get; set; }

    }
}