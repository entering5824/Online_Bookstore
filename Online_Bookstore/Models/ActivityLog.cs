using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Online_Bookstore.Models
{
    [Table("activity_logs")]

    public class ActivityLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("log_id")]
        public int LogId { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public int? UserId { get; set; }   // khoá ngoại

        public virtual User User { get; set; }  // navigation property

        [StringLength(255)]
        [Column("action")]
        public string Action { get; set; }

        [Column("log_time")]
        public DateTime? LogTime { get; set; }

        [StringLength(45)]
        [Column("ip_address")]
        public string IpAddress { get; set; }

    }
}