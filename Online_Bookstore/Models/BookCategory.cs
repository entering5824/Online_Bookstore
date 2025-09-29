using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Bookstore.Models
{
    [Table("book_categories")]
    public class BookCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("category_name")]
        public string CategoryName { get; set; }

        [Column("description", TypeName = "nvarchar(max)")]
        public string Description { get; set; }

        // Navigation property (một Category có nhiều Book)
        public virtual ICollection<Book> Books { get; set; }

        public BookCategory()
        {
            Books = new HashSet<Book>();
        }
    }
}
