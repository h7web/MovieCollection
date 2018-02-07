using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace herman.Models
{
    public partial class character
    {
        public string char_fullname
        {
            get
            {
                var fullname = "";

                fullname = char_first_name + ' ';
                if (char_mi != null)
                {
                    fullname = fullname + char_mi + ' ';
                }

                fullname = fullname + char_last_name;

                if (char_alias != null)
                {
                    fullname = fullname + ' ' + '(' + char_alias + ')';
                }

                return fullname;
            }
        }

    }
}