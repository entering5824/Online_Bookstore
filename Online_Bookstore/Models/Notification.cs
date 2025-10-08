using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Online_Bookstore.Models
{
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int NotificationId { get; set; }

        [ForeignKey("User")]
        [Column("UserId")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        [StringLength(20)]
        [Column("UserCode")]
        public string UserCode { get; set; }

        [Required]
        [StringLength(200)]
        [Column("Title")]
        public string Title { get; set; }

        [Required]
        [Column("Message")]
        public string Message { get; set; }

        [Required]
        [StringLength(20)]
        [Column("Type")]
        public string Type { get; set; }

        [Column("IsRead")]
        public bool IsRead { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("ReadDate")]
        public DateTime? ReadDate { get; set; }

        }
    }