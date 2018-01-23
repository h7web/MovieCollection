using herman.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Data.Entity.Validation;

namespace herman.Controllers
{
    public class HomeController : Controller
    {
        private Models.Entities db = new Models.Entities();
        public ActionResult Index(string search)
        {
            IQueryable<VideoViewModel> vlist;

            if (search != null)
            {
                vlist = from v in db.Videos
                        join d in db.directors on v.Director equals d.dir_id
                        where v.Video_Name.Contains(search)
                        select new VideoViewModel
                        {
                            video_id = v.video_id,
                            Video_Name = v.Video_Name,
                            VHS = v.VHS,
                            DVD = v.DVD,
                            BLURAY = v.BLURAY,
                            DIGITAL = v.DIGITAL,
                            Release_Date = v.Release_Date,
                            Plot = v.Plot,
                            Box_Cover = v.Box_Cover,
                            dir_first_name = d.dir_first_name,
                            dir_last_name = d.dir_last_name
                        };
            }
            else
            {
                vlist = from v in db.Videos
                        where v.featured == true
                        join d in db.directors on v.Director equals d.dir_id
                        select new VideoViewModel
                        {
                            video_id = v.video_id,
                            Video_Name = v.Video_Name,
                            VHS = v.VHS,
                            DVD = v.DVD,
                            BLURAY = v.BLURAY,
                            DIGITAL = v.DIGITAL,
                            Release_Date = v.Release_Date,
                            Plot = v.Plot,
                            Box_Cover = v.Box_Cover,
                            dir_first_name = d.dir_first_name,
                            dir_last_name = d.dir_last_name
                        };

            }

            return View(vlist);
        }

        public ActionResult VideoDetails(int id) 
        {
             var getvideo = (from v in db.Videos
                        where v.video_id == id
                        join d in db.directors on v.Director equals d.dir_id
                        join c in db.categories on v.Category equals c.category_id
                        select new VideoViewModel
                        {
                            video_id = v.video_id,
                            Video_Name = v.Video_Name,
                            VHS = v.VHS,
                            DVD = v.DVD,
                            BLURAY = v.BLURAY,
                            DIGITAL = v.DIGITAL,
                            Release_Date = v.Release_Date,
                            Plot = v.Plot,
                            Box_Cover = v.Box_Cover,
                            dir_first_name = d.dir_first_name,
                            dir_last_name = d.dir_last_name,
                            length = v.length,
                            rating = v.rating,
                            category_name = c.category_name
                        }).SingleOrDefault();

            var getcast = (from a2 in db.actor2movie where a2.video_id == id
                           join a in db.actors on a2.actor_id equals a.actor_id
                           join c in db.characters on a2.char_id equals c.char_id
                           select new CastList
                           {
                               actor_id = a2.actor_id,
                               actor_first_name = a == null ? "" : a.actor_first_name,
                               actor_mi = a == null ? "" : a.actor_mi,
                               actor_last_name = a == null ? "" : a.actor_last_name,
                               actor_suffix = a == null ? "" : a.actor_suffix,
                               actor_photo = a == null ? "" : a.actor_photo,
                               char_first_name = c == null ? "" : c.char_first_name,
                               char_mi = c == null ? "" : c.char_mi,
                               char_last_name = c == null ? "" : c.char_last_name,
                               char_alias = c == null ? "" : c.char_alias
    });

            getvideo.VideoCastList = getcast.ToList();

            return View(getvideo);
        }

        public ActionResult ViewActor(int id)
        {
            var getactor = (from a in db.actors where a.actor_id == id
                            select a).SingleOrDefault();

            getactor.mymovies = (from a2 in db.actor2movie
                                 where a2.actor_id == id
                                 join v in db.Videos on a2.video_id equals v.video_id
                                 join c in db.characters on a2.char_id equals c.char_id
                                 orderby v.Release_Date descending
                                 select new ActorVideo
                                 {
                                     video_id = v.video_id,
                                     Video_Name = v.Video_Name,
                                     Release_Date = v.Release_Date,
                                     char_first_name = c == null ? "" : c.char_first_name,
                                     char_mi = c == null ? "" : c.char_mi,
                                     char_last_name = c == null ? "" : c.char_last_name,
                                     char_alias = c == null ? "" : c.char_alias
                                 }).ToList();

            return View(getactor);
        }

        public List<SelectListItem> GetCategories(string selected = null)
        {
            List<SelectListItem> categories = new List<SelectListItem>();

            categories.Add(new SelectListItem { Value = "", Text = "* Select a Category" });

            var cats = (from d in db.categories orderby d.category_name select d).ToList().Distinct();

            foreach (var d in cats)
            {
                categories.Add(new SelectListItem { Value = d.category_id.ToString(), Text = d.category_name, Selected = (selected == d.category_id.ToString()) ? true : false });
            }

            return categories;
        }
        public List<SelectListItem> GetDirectors(string selected = null)
        {
            List<SelectListItem> directors = new List<SelectListItem>();

            directors.Add(new SelectListItem { Value = "", Text = "* Select Director" });

            var dirs = (from d in db.directors orderby d.dir_last_name, d.dir_first_name select d).ToList().Distinct();

            foreach (var d in dirs)
            {
                directors.Add(new SelectListItem { Value = d.dir_id.ToString(), Text = d.dir_first_name + ' ' + d.dir_last_name, Selected = (selected == d.dir_id.ToString()) ? true : false });
            }

            return directors;
        }

        public List<IMDBSearchResult> SearchResults(string search)
        {
            var jsonString = new WebClient().DownloadString("http://www.omdbapi.com/?apikey=b2c5d76a&r=json&type=movie&s=" + search);

            var imdbsearch = JsonConvert.DeserializeObject<Results>(jsonString);

            return imdbsearch.Search;
        }

        public ActionResult SearchForVideo(string search)
        {
            var ret = SearchResults(search);

            return PartialView(ret);
        }

        public IMDBSearchResult GetIMDBVideoDetails(string id)
        {
            var jsonString = new WebClient().DownloadString("http://www.omdbapi.com/?apikey=b2c5d76a&r=json&i=" + id);

            var imdbsearch = JsonConvert.DeserializeObject<IMDBSearchResult>(jsonString);

            return imdbsearch;
        }

        public ActionResult GetVideoDetails(string id)
        {
            var ret = GetIMDBVideoDetails(id);

            return PartialView(ret);
        }

        public ActionResult AddVideo()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddVideo(VideoViewModel vid)
        {
            var dir = new director();

            int rating = 0;

            if (vid.ratingtxt == "PG")
            {
                rating = 2;
            }
            else if (vid.ratingtxt == "PG-13")
            {
                rating = 3;
            }
            else if (vid.ratingtxt == "R")
            {
                rating = 4;
            }
            else
            {
                rating = 1;
            }

            var len = vid.lengthtxt.Length;
            len = len - 4;
            int length = Int32.Parse(vid.lengthtxt.Substring(0, len));

            var newVideo = new Video();

            newVideo.Video_Name = vid.Video_Name;
            newVideo.VHS = vid.VHS;
            newVideo.DVD = vid.DVD;
            newVideo.BLURAY = vid.BLURAY;
            newVideo.DIGITAL = vid.DIGITAL;
            newVideo.rating = rating;
            newVideo.length = length;
            newVideo.Plot = vid.Plot;
            newVideo.Director = vid.Director;
            newVideo.Category = vid.Category;
            newVideo.Box_Cover = vid.Box_Cover;


            try
            {
                db.Videos.Add(newVideo);

                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Console.Write("err is " + ex);
            }

            return RedirectToAction("Index");
        }
    }
}