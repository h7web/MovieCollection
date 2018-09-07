using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace herman.Models
{
    public class IMDBSearchResult
    {
        public string Title { get; set; }

        public string Year { get; set; }

        public string imdbID { get; set; }

        public string Type { get;set; }

        public string Poster { get; set; }

        public string Plot { get; set; }

        public string Rated { get; set; }

        public string Runtime { get; set; }

        public string Genre { get; set; }

        public string Director { get; set; }

        public int dir_id { get; set; }

        public string id { get; set; }

    }
}