using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Online_Bookstore.Models
{
    [Table("borrow_records")]

    public class BorrowRecord
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Column("record_id")]
            public int RecordId { get; set; }

            [ForeignKey("User")]
            [Column("user_id")]
            public int? UserId { get; set; }
            public virtual User User { get; set; }

            [StringLength(20)]
            [Column("user_code")]
            public string UserCode { get; set; }

            [ForeignKey("Book")]
            [Column("book_id")]
            public int? BookId { get; set; }
            public virtual Book Book { get; set; }

            [StringLength(20)]
            [Column("book_code")]
            public string BookCode { get; set; }

            [Column("borrow_date")]
            public DateTime? BorrowDate { get; set; }

            [Column("due_date")]
            public DateTime? DueDate { get; set; }

            [Column("return_date")]
            public DateTime? ReturnDate { get; set; }

            [StringLength(20)]
            [Column("status")]
            public string Status { get; set; }

            [Column("fine_amount")]
            public double? FineAmount { get; set; }

        }
    }