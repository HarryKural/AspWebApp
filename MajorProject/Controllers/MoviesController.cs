using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MajorProject.Models;

namespace MajorProject.Controllers
{
    public class MoviesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Movies
        public ActionResult Index()
        {
            var movies = db.Movies;
            return View(movies.ToList());
        }

        // GET: Movies/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);

            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            Movie model = new Movie();
            model.Name = String.Format("Movie - {0}", DateTime.Now.Ticks);

            ViewBag.Actors = new MultiSelectList(db.Actors.ToList(), "ActorId", "Name", model.Actors.Select(x => x.ActorId).ToArray());
            return View(model);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,HitMovie,ActorIds")] Movie movie, string[] ActorIds)
        {
            if (ModelState.IsValid)
            {
                Movie checkmovie = db.Movies.SingleOrDefault(x => x.Name == movie.Name && x.HitMovie == movie.HitMovie);

                if(checkmovie == null)
                {
                    db.Movies.Add(movie);
                    db.SaveChanges();

                    if(ActorIds != null)
                    {
                        foreach (string actorId in ActorIds)
                        {
                            MovieActor movieActor = new MovieActor();

                            movieActor.MovieId = movie.MovieId;
                            movieActor.ActorId = actorId;
                            movie.Actors.Add(movieActor);
                        }

                        db.Entry(movie).State = EntityState.Modified;

                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }

                else
                {
                    ModelState.AddModelError("", "Duplicated Movie detected.");
                }
            }

            ViewBag.Actors = new MultiSelectList(db.Actors.ToList(), "ActorId", "Name", ActorIds);

            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie model = db.Movies.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.Actors = new MultiSelectList(db.Actors.ToList(), "ActorId", "Name", model.Actors.Select(x => x.ActorId).ToArray());

            return View(model);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieId,Name,HitMovie,ActorIds")] Movie movie, string[] ActorIds)
        {
            if (ModelState.IsValid)
            {
                Movie newMovie = db.Movies.Find(movie.MovieId);
                if(newMovie != null)
                {
                    Movie checkmovie = db.Movies.SingleOrDefault(
                                        x => x.Name == movie.Name &&
                                        x.HitMovie == movie.HitMovie &&
                                        x.MovieId != movie.MovieId);

                    if(checkmovie == null)
                    {
                        newMovie.Name = movie.Name;
                        newMovie.HitMovie = movie.HitMovie;
                        newMovie.CreateDate = DateTime.Now;

                        db.Entry(newMovie).State = EntityState.Modified;

                        //Items to remove
                        var removeItems = newMovie.Actors.Where(x => !ActorIds.Contains(x.ActorId)).ToList();

                        foreach (var removeItem in removeItems)
                        {
                            db.Entry(removeItem).State = EntityState.Deleted;
                        }

                        if (ActorIds != null)
                        {
                            var addedItems = ActorIds.Where(x => !newMovie.Actors.Select(y => y.ActorId).Contains(x));

                            //Items to add
                            foreach (string addedItem in addedItems)
                            {
                                MovieActor movieActor = new  MovieActor();

                                movieActor.MovieActorId = Guid.NewGuid().ToString();
                                movieActor.CreateDate = DateTime.Now;
                                movieActor.EditDate = movieActor.CreateDate;

                                movieActor.MovieId = newMovie.MovieId;
                                movieActor.ActorId = addedItem;
                                db.MovieActors.Add(movieActor);
                            }
                        }

                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    else
                    {
                        ModelState.AddModelError("", "Duplicated Movie detected.");
                    }
                }
                
            }
            ViewBag.Actors = new MultiSelectList(db.Actors.ToList(), "ActorId", "Name", ActorIds);

            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Movie movie = db.Movies.Find(id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            foreach (var item in movie.Actors.ToList())
            {
                db.MovieActors.Remove(item);
            }

            db.Movies.Remove(movie);

            var deleted = db.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
