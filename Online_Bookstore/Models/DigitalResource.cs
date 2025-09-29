using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Online_Bookstore.Models
{
    [Table("digital_resources")]

    public class DigitalResource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("resource_id")]
        public int ResourceId { get; set; }

        [ForeignKey("Book")]
        [Column("book_id")]
        public int? BookId { get; set; }
        public virtual Book Book { get; set; }

        [StringLength(20)]
        [Column("book_code")]
        public string BookCode { get; set; }

        [Required]
        [StringLength(255)]
        [Column("title")]
        public string Title { get; set; }

        [StringLength(100)]
        [Column("format")]
        public string Format { get; set; }

        [Column("file_size")]
        public long? FileSize { get; set; }

        [StringLength(255)]
        [Column("file_url")]
        public string FileUrl { get; set; }

        [Column("upload_date")]
        public DateTime? UploadDate { get; set; }

        [StringLength(20)]
        [Column("status")]
        public string Status { get; set; }

    }
}