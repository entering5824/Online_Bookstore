using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Online_Bookstore.Models
{
    [Table("Reservations")]
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Column("UserId")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Column("UserCode")]
        [StringLength(20)]
        public string UserCode { get; set; }

        [ForeignKey("Book")]
        [Column("BookId")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        [Column("BookCode")]
        [StringLength(20)]
        public string BookCode { get; set; }

        [Column("ReservationDate")]
        public DateTime ReservationDate { get; set; }

        [Column("ExpiryDate")]
        public DateTime ExpiryDate { get; set; }

        [Column("Status")]
        [StringLength(20)]
        public string Status { get; set; }

        [Column("Notes")]
        public string Notes { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }

    }
}