using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_Bookstore.Models
{
    [Table("books")]

    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("book_id")]
        public int BookId { get; set; }

        [Column("book_code")]
        [StringLength(20)]
        public string BookCode { get; set; }

        [Required]
        [StringLength(255)]
        [Column("title")]
        public string Title { get; set; }

        [StringLength(100)]
        [Column("author")]
        public string Author { get; set; }

        [Column("publication_year")]
        public int? PublicationYear { get; set; }

        [StringLength(50)]
        [Column("isbn")]
        public string Isbn { get; set; }

        [Column("total_copies")]
        public int? TotalCopies { get; set; }

        [Column("available_copies")]
        public int? AvailableCopies { get; set; }

        [ForeignKey("Category")]
        [Column("category_id")]
        public int? CategoryId { get; set; }

        public virtual BookCategory Category { get; set; }

        [StringLength(50)]
        [Column("publisher")]
        public string Publisher { get; set; }

        [StringLength(20)]
        [Column("language")]
        public string Language { get; set; }

        [StringLength(500)]
        [Column("description")]
        public string Description { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [StringLength(255)]
        [Column("cover_image_url")]
        public string CoverImageUrl { get; set; }

        [Column("is_digital")]
        public bool? IsDigital { get; set; }

        [StringLength(255)]
        [Column("qr_code")]
        public string QrCode { get; set; }

    }
}