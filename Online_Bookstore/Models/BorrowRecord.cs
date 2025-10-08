using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Online_Bookstore.Models
{
    [Table("BorrowRecords")]
    public class BorrowRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Column("UserId")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [StringLength(20)]
        [Column("UserCode")]
        public string UserCode { get; set; }

        [ForeignKey("Book")]
        [Column("BookId")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        [StringLength(20)]
        [Column("BookCode")]
        public string BookCode { get; set; }

        [Column("BorrowDate")]
        public DateTime BorrowDate { get; set; }

        [Column("DueDate")]
        public DateTime DueDate { get; set; }

        [Column("ReturnDate")]
        public DateTime? ReturnDate { get; set; }

        [StringLength(20)]
        [Column("Status")]
        public string Status { get; set; }

        [Column("Notes")]
        public string Notes { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }

        }
    }