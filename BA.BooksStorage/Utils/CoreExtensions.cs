using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BA.BooksStorage.Model.Model;

namespace BA.BooksStorage.Utils
{
    public static class CoreExtensions
    {
        public static void AddHit(this Book book)
        {
            var hit = book.Hits.FirstOrDefault(x => x.Date == DateTime.UtcNow.Date);
            if (hit == null)
            {
                book.Hits.Add(new Hit { Date = DateTime.UtcNow.Date, Count = 1 });
            }
            else
            {
                hit.Count++;
            }
        }
    }
}