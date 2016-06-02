using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BA.BooksStorage.Model.Model;

namespace BA.BooksStorage.Model
{
    public class BooksStorageContext : DbContext
    {
        public BooksStorageContext()
            : base("name=BooksStorageDb")
        {
            Database.SetInitializer(new BooksStorageContextSeedInitializer());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ShopDbContext>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<BooksStorageContext, Configuration>());
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Hit> Hits { get; set; }
    }

    public class BooksStorageContextSeedInitializer : DropCreateDatabaseAlways<BooksStorageContext>
    {
        protected override void Seed(BooksStorageContext db)
        {
            Random r = new Random();

            IEnumerable<Author> authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "Andrew", LastName = "Hunt" },
                new Author { Id = 2, FirstName = "Andrew", LastName = "Troelsen" },
                new Author { Id = 3, FirstName = "Erich", LastName = "Gamma" },
                new Author { Id = 4, FirstName = "Thomas", LastName = "Cormen" },
            };

            IEnumerable<Book> books = new List<Book>
            {
                new Book { Id = 1, Title = "The Pragmatic Programmer: From Journeyman to Master", Isbn = "2-266-11156-4", Author = authors.FirstOrDefault(a => a.Id == 1)},
                new Book { Id = 2, Title = "Pragmatic Unit Testing in Java with JUnit", Isbn = "7-432-76524-9", Author = authors.FirstOrDefault(a => a.Id == 1)},
                new Book { Id = 3, Title = "Pro C# 5.0 and the .NET 4.5 Framework Sixth Edition", Isbn = "9-351-76520-2", Author = authors.FirstOrDefault(a => a.Id == 2)},
                new Book { Id = 4, Title = "Design Patterns: Elements of Reusable Object-Oriented Software", Isbn = "6-986-12541-3", Author = authors.FirstOrDefault(a => a.Id == 3)},
                new Book { Id = 5, Title = "Refactoring: Improving the Design of Existing Code", Isbn = "8-619-38751-1", Author = authors.FirstOrDefault(a => a.Id == 3)},
                new Book { Id = 6, Title = "Introduction to Algorithms", Isbn = "5-812-92341-2", Author = authors.FirstOrDefault(a => a.Id == 4)},
            };

            IList<Hit> hits = new List<Hit>();
            int currentId = 0;
            foreach (var book in books)
            {
                for (var day = 0; day < 30; day++)
                {
                    hits.Add(new Hit
                    {
                        Id = currentId,
                        Date = DateTime.UtcNow.Date.AddDays(-1 * day),
                        Count = r.Next(1, 100),
                        Book = book
                    });
                    currentId++;
                }
            }

            db.Authors.AddRange(authors);
            db.Books.AddRange(books);
            db.Hits.AddRange(hits);
            db.SaveChanges();
        }
    }
}
