using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Online_Bookstore.Models
{
    [Table("DigitalResources")]
    public class DigitalResource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int ResourceId { get; set; }

        [Required]
        [StringLength(200)]
        [Column("Title")]
        public string Title { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Required]
        [StringLength(500)]
        [Column("FileUrl")]
        public string FileUrl { get; set; }

        [Required]
        [StringLength(20)]
        [Column("FileType")]
        public string FileType { get; set; }

        [Column("FileSize")]
        public long? FileSize { get; set; }

        [Column("Category")]
        public string Category { get; set; }

        [Column("Tags")]
        public string Tags { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; }

        [Column("DownloadCount")]
        public int DownloadCount { get; set; }

        [ForeignKey("Book")]
        [Column("BookId")]
        public int? BookId { get; set; }
        public virtual Book Book { get; set; }

        [StringLength(20)]
        [Column("BookCode")]
        public string BookCode { get; set; }

        [StringLength(20)]
        [Column("AccessLevel")]
        public string AccessLevel { get; set; }

    }
}