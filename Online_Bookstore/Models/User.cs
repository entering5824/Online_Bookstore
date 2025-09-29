using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Online_Bookstore.Models
{
[Table("users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("user_id")]
    public int UserId { get; set; }

    [StringLength(20)]
    [Column("user_code")]
    public string UserCode { get; set; }

    [Required]
    [StringLength(50)]
    [Column("username")]
    public string Username { get; set; }

    [Required]
    [StringLength(255)]
    [Column("password_hash")]
    public string PasswordHash { get; set; }

    [Required]
    [StringLength(100)]
    [Column("full_name")]
    public string FullName { get; set; }

    [Required]
    [StringLength(100)]
    [Column("email")]
    public string Email { get; set; }

    [Required]
    [StringLength(10)]
    [Column("role")]
    public string Role { get; set; }

    [StringLength(10)]
    [Column("status")]
    public string Status { get; set; }

    [StringLength(10)]
    [Column("language_preference")]
    public string LanguagePreference { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
}}