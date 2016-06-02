using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BA.BooksStorage.Model;
using BA.BooksStorage.Model.Model;

namespace BA.BooksStorage.Controllers
{
    public class AuthorsController : Controller
    {
        BooksStorageContext _db = new BooksStorageContext();

        // GET: Authors
        [OutputCache(Duration = 120, 
            Location = System.Web.UI.OutputCacheLocation.Server)]
        public ActionResult Index()
        {
            IEnumerable<Author> authors = _db.Authors.OrderBy(a => a.LastName).ToList();
            return View(authors);
        }

        public ActionResult IndexRemoveCash()
        {
            var path = Url.Action("Index", "Authors");
            Response.RemoveOutputCacheItem(path);
            return RedirectToAction("Index");
        }

        // GET: Authors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        [HttpPost]
        public ActionResult Create(Author author)
        {
            if (ModelState.IsValid)
            {
                _db.Authors.Add(author);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Authors/Edit/5
        public ActionResult Edit(int id)
        {
            Author author = _db.Authors.FirstOrDefault(x => x.Id == id);
            return View(author);
        }

        // POST: Authors/Edit/5
        [HttpPost]
        public ActionResult Edit(Author author)
        {
            try
            {
                _db.Entry(author).State = EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(author);
            }
        }

        // GET: Authors/Delete/5
        public ActionResult Delete(int id)
        {
            Author author = _db.Authors.FirstOrDefault(b => b.Id == id);
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost]
        public ActionResult Delete(Author author)
        {
            try
            {
                var countBooks = _db.Books.Count(b => b.Author.Id == author.Id);
                if (countBooks != 0)
                {
                    ModelState.AddModelError("AuthorHasBooks", $"This author has {countBooks} book(s). Firstly delete this book(s).");
                    return View(author);
                   // return RedirectToAction("Delete", new { id = author.Id});
                }

                _db.Entry(author).State = EntityState.Deleted;
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(author);
            }
        }

        // Check and make sure, that all connections to the database were closed
        // and memory was exempted 
        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
            {
                this._db.Dispose();
            }

            base.Dispose(Disposing);
        }
    }
}
