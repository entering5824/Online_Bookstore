using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Online_Bookstore.Models
{
    [Table("reservations")]

    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("reservation_id")]
        public int ReservationId { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        [Column("user_code", TypeName = "nvarchar(20)")]
        [MaxLength(20)]
        public string UserCode { get; set; }

        [ForeignKey("Book")]
        [Column("book_id")]
        public int? BookId { get; set; }
        public virtual Book Book { get; set; }

        [Column("book_code", TypeName = "nvarchar(20)")]
        [MaxLength(20)]
        public string BookCode { get; set; }

        [Column("reservation_date")]
        public DateTime? ReservationDate { get; set; }

        [Column("expiry_date")]
        public DateTime? ExpiryDate { get; set; }

        [Column("status", TypeName = "nvarchar(20)")]
        [MaxLength(20)]
        public string Status { get; set; }

    }
}