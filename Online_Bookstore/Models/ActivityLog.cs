using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Online_Bookstore.Models
{
    [Table("ActivityLogs")]
    public class ActivityLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int LogId { get; set; }

        [ForeignKey("User")]
        [Column("UserId")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Action")]
        public string Action { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [StringLength(45)]
        [Column("IpAddress")]
        public string IpAddress { get; set; }

        [Column("UserAgent")]
        public string UserAgent { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

    }
}