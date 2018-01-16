    public class FormatHelper
    {
        public static string GetMediaFormat(bool? val, string format)
        {
            string displayimg = "";

            if (val==true)
            {
                if (format == "VHS")
                {
                    displayimg = "<img src=http://h7.hopto.org/herman/images/VHS.gif class=formats />";
                }
                else if (format == "DVD")
                {
                    displayimg = "<img src=http://h7.hopto.org/herman/images/DVD.gif class=formats />";
                }
                else if (format == "BLURAY")
                {
                    displayimg = "<img src=http://h7.hopto.org/herman/images/bluray_logo.jpg class=formats />";
                }
                else if (format == "DIGITAL")
                {
                    displayimg = "<img src=http://h7.hopto.org/herman/images/digital_logo.jpg class=formats />";
                }
            }

            return (displayimg);
        }

    public static string GetRatingFormat(int? val)
    {
        string displayrating = "";

        if(val == 1)
        {
            displayrating = "<img src=http://h7.hopto.org/herman/images/g.jpg />";
        }
        else if (val == 2)
        {
            displayrating = "<img src=http://h7.hopto.org/herman/images/pg.jpg />";
        }
        else if (val == 3)
        {
            displayrating = "<img src=http://h7.hopto.org/herman/images/pg-13.jpg />";
        }
        else if (val == 4)
        {
            displayrating = "<img src=http://h7.hopto.org/herman/images/r.jpg />";
        }

        return (displayrating);
    }
    }
