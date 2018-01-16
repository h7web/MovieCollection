using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace herman.Models
{
    [DataContract]
    public class IMDBSearchResult
    {
        [DataMember]
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        [DataMember]
        private string _Year;
        public string Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        [DataMember]
        private string _imdbID;
        public string imdbID
        {
            get { return _imdbID; }
            set { _imdbID = value; }
        }

        [DataMember]
        private string _Type;
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        [DataMember]
        private string _Poster;
        public string Poster
        {
            get { return _Poster; }
            set { _Poster = value; }
        }

        public IMDBSearchResult(string Title, string Year, string imdbID, string Type, string Poster)
        {
            _Title = Title;
            _Year = Year;
            _imdbID = imdbID;
            _Type = Type;
            _Poster = Poster;
        }
    }
}