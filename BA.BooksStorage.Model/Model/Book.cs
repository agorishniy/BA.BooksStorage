using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BA.BooksStorage.Model.Model
{
    public class Book
    {
        public Book()
        {
            Hits = new HashSet<Hit>();
        }

        [Key]
        [Column("BookId")]
        public int Id { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Title must be between 2 and 150", MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [DisplayName("ISBN")]
        [RegularExpression(@"(?=.{13}$)\d{1,5}([- ])\d{1,7}\1\d{1,6}\1(\d|X)$", ErrorMessage = "Invalid ISBN!")]
        public string Isbn { get; set; }

        [Required]
        public virtual Author Author { get; set; }

        public virtual ICollection<Hit> Hits { get; set; }
    }
}
