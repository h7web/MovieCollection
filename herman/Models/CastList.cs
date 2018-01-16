using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace herman.Models
{
    public class CastList
    {
        public int actor_id { get; set; }
        public string actor_first_name { get; set; }
        public string actor_mi { get; set; }
        public string actor_last_name { get; set; }
        public string actor_suffix { get; set; }
        public string actor_photo { get; set; }

        public string char_first_name { get; set; }
        public string char_mi { get; set; }
        public string char_last_name { get; set; }
        public string char_alias { get; set; }
    }
}