using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace herman.Models
{
    public class VideoViewModel
    {
        public int video_id { get; set; }
        public string Video_Name { get; set; }
        public Nullable<bool> VHS { get; set; }
        public Nullable<bool> DVD { get; set; }
        public Nullable<bool> BLURAY { get; set; }
        public Nullable<bool> DIGITAL { get; set; }
        public Nullable<int> Release_Date { get; set; }
        public string Plot { get; set; }
        public string Box_Cover { get; set; }
        public string dir_first_name { get; set; }
        public string dir_last_name { get; set; }
   
        public Nullable<int> length { get; set; }
        public Nullable<int> rating { get; set; }
        public string category_name { get; set; }

        public List<CastList> VideoCastList { get; set; }

    }
}