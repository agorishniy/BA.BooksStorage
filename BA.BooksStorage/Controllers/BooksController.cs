using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BA.BooksStorage.Model;
using BA.BooksStorage.Model.Model;
using BA.BooksStorage.Utils;

namespace BA.BooksStorage.Controllers
{
    public class BooksController : Controller
    {
        BooksStorageContext _db = new BooksStorageContext();

        // GET: Books
        public ActionResult Index()
        {
            IEnumerable<Book> books = _db.Books.OrderBy(b => b.Author.LastName).ToList();
            return View(books);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.Authors = _db.Authors.Select(a => new SelectListItem
                                                {
                                                    Text = a.LastName + " " + a.FirstName,
                                                    Value = a.Id.ToString()
                                                })
                                          .OrderBy(a => a.Text).ToList();
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        public ActionResult Create(Book book)
        {
            int authorId = int.Parse(Request.Params["Authors"]);
            book.Author = _db.Authors.FirstOrDefault(a => a.Id == authorId);

            _db.Books.Add(book);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            Book book = _db.Books.FirstOrDefault(x => x.Id == id);
            book.AddHit();
            _db.Books.AddOrUpdate(book);
            _db.SaveChanges();

            var avgHits = _db.Books.SelectMany(b => b.Hits)
                            .GroupBy(x => x.Date)
                            .Select(s => new HitChart
                            {
                                Date = s.Key,
                                Count = s.Average(d => d.Count)
                            }).ToList();

            ViewBag.GoogleChartData = HitUtils.GoogleChartData(book.Hits, avgHits);

            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int id)
        {
            Book book = _db.Books.FirstOrDefault(b => b.Id == id);
            ViewBag.Authors = _db.Authors.Select(a => new SelectListItem
                                {
                                    Text = a.LastName + " " + a.FirstName,
                                    Value = a.Id.ToString()
                                })
                                .OrderBy(a => a.Text);

            return View(book);
        }

        // POST: Books/Edit
        [HttpPost]
        public ActionResult Edit(int id, Book book)
        {
            try
            {
                Book b = _db.Books.FirstOrDefault(x => x.Id == id);
                b.Title = book.Title;
                b.Isbn = book.Isbn;
                b.Author = _db.Authors.FirstOrDefault(x => x.Id == book.Author.Id);

                _db.Books.AddOrUpdate(b);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Books/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Book book = _db.Books.FirstOrDefault(x => x.Id == id);
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Book book)
        {
            try
            {
                Book b = _db.Books.FirstOrDefault(x => x.Id == id);
               // IEnumerable<Hit> hits = _db.Hits.Where(x => x.Book.Id == id).ToList();

                //_db.Hits.RemoveRange(hits);
                _db.Books.Remove(b);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
