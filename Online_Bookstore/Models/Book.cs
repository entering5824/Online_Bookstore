using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_Bookstore.Models
{
    [Table("Books")]

    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int BookId { get; set; }

        [Column("BookCode")]
        [StringLength(20)]
        public string BookCode { get; set; }

        [Required]
        [StringLength(255)]
        [Column("Title")]
        public string Title { get; set; }

        [StringLength(100)]
        [Column("Author")]
        public string Author { get; set; }

        [Column("PublicationYear")]
        public int? PublicationYear { get; set; }

        [StringLength(50)]
        [Column("ISBN")]
        public string Isbn { get; set; }

        [Column("Quantity")]
        public int? TotalCopies { get; set; }

        [Column("AvailableQuantity")]
        public int? AvailableCopies { get; set; }

        [ForeignKey("Category")]
        [Column("CategoryId")]
        public int? CategoryId { get; set; }

        public virtual BookCategory Category { get; set; }

        [StringLength(50)]
        [Column("Publisher")]
        public string Publisher { get; set; }

        [StringLength(20)]
        [Column("Language")]
        public string Language { get; set; }

        [StringLength(500)]
        [Column("Description")]
        public string Description { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }

        [StringLength(255)]
        [Column("CoverImageUrl")]
        public string CoverImageUrl { get; set; }

        [Column("IsDigital")]
        public bool? IsDigital { get; set; }

        [StringLength(255)]
        [Column("QRCode")]
        public string QrCode { get; set; }

    }
}