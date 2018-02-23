using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace herman.Models
{
    public class Dashboard
    {
        public IEnumerable<VideoViewModel> Vmm { get; set; }

        public int CountVHS { get; set; }
        public int CountDVD { get; set; }
        public int CountBluray { get; set; }
        public int CountDIGITAL { get; set; }
        public int CountTotal { get; set; }

        public List<top5s> Top5 { get; set; }
    }
}