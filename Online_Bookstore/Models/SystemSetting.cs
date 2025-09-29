using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Online_Bookstore.Models
{
    [Table("system_settings")]

    public class SystemSetting
    {
        [Key]
        [Column("setting_key", TypeName = "nvarchar(100)")]
        [MaxLength(100)]
        public string SettingKey { get; set; }

        [Column("setting_value", TypeName = "nvarchar(max)")]
        public string SettingValue { get; set; }

    }
}