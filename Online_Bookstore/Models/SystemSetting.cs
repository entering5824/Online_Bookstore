using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Online_Bookstore.Models
{
    [Table("SystemSettings")]
    public class SystemSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("SettingKey")]
        public string SettingKey { get; set; }

        [Column("SettingValue")]
        public string SettingValue { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Category")]
        public string Category { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; }

    }
}