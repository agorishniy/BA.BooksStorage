using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BA.BooksStorage.Model.Model
{
    public class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        [Key]
        [Column("AuthorId")]
        public int Id { get; set; }

        [Required]
        [DisplayName("First name")]
        [StringLength(50, ErrorMessage = "First name must be between 2 and 50", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last name")]
        [StringLength(150, ErrorMessage = "Last name must be between 2 and 150", MinimumLength = 2)]
        public string LastName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
