using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace herman.Models
{
    public class Results
    {
        public List<IMDBSearchResult> Search { get; set; }

        public string totalResults { get; set; }

        public string Response { get; set; }
    }
}