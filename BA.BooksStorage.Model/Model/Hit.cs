using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BA.BooksStorage.Model.Model
{
    public class Hit
    {
        [Key]
        [Column("HitId")]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public virtual Book Book { get; set; }
    }
}
