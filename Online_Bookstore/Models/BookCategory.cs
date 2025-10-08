using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Bookstore.Models
{
    [Table("BookCategories")]
    public class BookCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Name")]
        public string CategoryName { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; }

        // Navigation property (một Category có nhiều Book)
        public virtual ICollection<Book> Books { get; set; }

        public BookCategory()
        {
            Books = new HashSet<Book>();
        }
    }
}
