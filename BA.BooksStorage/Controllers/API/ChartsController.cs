using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using BA.BooksStorage.Model;
using BA.BooksStorage.Model.Model;
using BA.BooksStorage.Utils;

namespace BA.BooksStorage.Controllers.API
{
    public class ChartsController : ApiController
    {
        BooksStorageContext _db = new BooksStorageContext();

        [HttpGet]
        public HttpResponseMessage Hits(int id)
        {
            //Thread.Sleep(2000);

            Book book = _db.Books.FirstOrDefault(x => x.Id == id);

            if (book == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Book is missed");
            }

            var avgHits = _db.Books.SelectMany(b => b.Hits)
                            .GroupBy(x => x.Date)
                            .Select(s => new HitChart
                            {
                                Date = s.Key,
                                Count = s.Average(d => d.Count)
                            }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK,
                                          HitUtils.GoogleChartData(book.Hits, avgHits));
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
