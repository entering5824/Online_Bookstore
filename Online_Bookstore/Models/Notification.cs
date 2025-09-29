using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Online_Bookstore.Models
{
    [Table("notifications")]

    public class Notification
    {
 
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Column("notification_id")]
            public int NotificationId { get; set; }

            [ForeignKey("User")]
            [Column("user_id")]
            public int? UserId { get; set; }
            public virtual User User { get; set; }

            [StringLength(20)]
            [Column("user_code")]
            public string UserCode { get; set; }

            [Required]
            [StringLength(255)]
            [Column("message")]
            public string Message { get; set; }

            [StringLength(20)]
            [Column("type")]
            public string Type { get; set; }

            [Column("is_read")]
            public bool? IsRead { get; set; }

            [Column("created_at")]
            public DateTime? CreatedAt { get; set; }

        }
    }