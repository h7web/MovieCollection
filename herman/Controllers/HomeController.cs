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
                               actor = a,
                               character = c
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
        public ActionResult ViewDirector(int id)
        {
            var getdir = (from d in db.directors
                            where d.dir_id == id
                            select d).SingleOrDefault();

            getdir.mymovies = (from v in db.Videos where v.Director == id
                                 orderby v.Release_Date descending
                                 select new ActorVideo
                                 {
                                     video_id = v.video_id,
                                     Video_Name = v.Video_Name,
                                     Release_Date = v.Release_Date
                                 }).ToList();

            return View(getdir);
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

            var dirs = (from d in db.directors orderby d.dir_last_name, d.dir_first_name select d).ToList();

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

        public int FindDir(string val)
        {
            int dirid = 0;
            var fn = val.Substring(0, val.IndexOf(" "));
            var ln = (val.Substring((val.IndexOf(" ")), (val.Length - val.IndexOf(" ")))).TrimStart(' ');


            var dir = (from d in db.directors where d.dir_first_name == fn && d.dir_last_name == ln select d).FirstOrDefault();

            if (dir != null)
            {
                dirid = dir.dir_id;
            }

            return dirid;
        }

        public IMDBSearchResult GetIMDBVideoDetails(string id)
        {
            var jsonString = new WebClient().DownloadString("http://www.omdbapi.com/?apikey=b2c5d76a&r=json&i=" + id);

            var imdbsearch = JsonConvert.DeserializeObject<IMDBSearchResult>(jsonString);

            imdbsearch.dir_id = FindDir(imdbsearch.Director);

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

            if (vid.Director == null)
            {
                dir.dir_first_name = vid.newDirector.Substring(0, vid.newDirector.IndexOf(" "));
                dir.dir_last_name = (vid.newDirector.Substring((vid.newDirector.IndexOf(" ")), (vid.newDirector.Length - vid.newDirector.IndexOf(" ")))).Trim();

                db.directors.Add(dir);
                db.SaveChanges();

            }


            var newVideo = new Video();

            newVideo.Video_Name = vid.Video_Name;
            newVideo.VHS = vid.VHS;
            newVideo.DVD = vid.DVD;
            newVideo.BLURAY = vid.BLURAY;
            newVideo.DIGITAL = vid.DIGITAL;
            newVideo.Release_Date = vid.Release_Date;
            newVideo.rating = rating;
            newVideo.length = length;
            newVideo.Plot = vid.Plot;
            newVideo.Director = dir.dir_id;
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

                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public List<SelectListItem> GetActors(string selected = null)
        {
            List<SelectListItem> actors = new List<SelectListItem>();

            actors.Add(new SelectListItem { Value = "", Text = "* Select Actor" });

            var actrs = (from a in db.actors orderby a.actor_last_name, a.actor_first_name select a).ToList();

            foreach (var a in actrs)
            {
                actors.Add(new SelectListItem { Value = a.actor_id.ToString(), Text = a.actor_fullname, Selected = (selected == a.actor_id.ToString()) ? true : false });
            }

            return actors;
        }

        public List<SelectListItem> GetChars(string selected = null)
        {
            List<SelectListItem> chars = new List<SelectListItem>();

            chars.Add(new SelectListItem { Value = "", Text = "* Select Character" });

            var chrs = (from c in db.characters orderby c.char_last_name, c.char_first_name select c).ToList();

            foreach (var c in chrs)
            {
                chars.Add(new SelectListItem { Value = c.char_id.ToString(), Text = c.char_fullname, Selected = (selected == c.char_id.ToString()) ? true : false });
            }

            return chars;
        }

        public ActionResult AddActors(int id)
        {
            var getvideo = (from v in db.Videos
                            where v.video_id == id
                            join d in db.directors on v.Director equals d.dir_id
                            join c in db.categories on v.Category equals c.category_id
                            select new VideoViewModel
                            {
                                video_id = v.video_id,
                                Video_Name = v.Video_Name,
                                Release_Date = v.Release_Date
                            }).SingleOrDefault();

            return View(getvideo);
        }

        [HttpPost]
        public ActionResult AddActors(int id, actor a, character c)
        {
            actor2movie a2m = new actor2movie();

            if (a.actor_id == 0)
            {
                db.actors.Add(a);
                db.SaveChanges();
            }

            if (c.char_id == 0)
            {
                db.characters.Add(c);
                db.SaveChanges();
            }

            a2m.video_id = id;
            a2m.actor_id = a.actor_id;
            a2m.char_id = c.char_id;

            db.actor2movie.Add(a2m);
            db.SaveChanges();

            return RedirectToAction("VideoDetails", new { @id = id });
        }

        public ActionResult RemoveActor(int video_id, int actor_id, int char_id)
        {
            actor2movie a2m = (from a2 in db.actor2movie where a2.video_id == video_id && a2.actor_id == actor_id && a2.char_id == char_id select a2).SingleOrDefault();

            db.actor2movie.Remove(a2m);

            var ainm = from a2 in db.actor2movie where a2.actor_id == a2m.actor_id select a2;

            if (ainm.Count() < 2)
            {
                var actor = (from a in db.actors where a.actor_id == a2m.actor_id select a).FirstOrDefault();
                db.actors.Remove(actor);
            }

            var cinm = from a2 in db.actor2movie where a2.char_id == a2m.char_id select a2;

            if (cinm.Count() < 2)
            {
                var chr = (from c in db.characters where c.char_id == a2m.char_id select c).FirstOrDefault();
                db.characters.Remove(chr);
            }

            db.SaveChanges();

            return RedirectToAction("VideoDetails", new { @id = a2m.video_id });
        }
    }
}