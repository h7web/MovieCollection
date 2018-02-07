using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace herman.Models
{
    public partial class actor
    {
        public List<ActorVideo> mymovies { get; set; }

        public string actor_fullname
        {
            get
            {
                var fullname = "";

                fullname = actor_first_name + ' ';
                if (actor_mi != null)
                {
                    fullname = fullname + actor_mi + ' ';
                }

                fullname = fullname + actor_last_name;
          
                if(actor_suffix != null)
                {
                    fullname = fullname + ' ' + actor_suffix;
                }

                return fullname;
            }
        }
    }
}