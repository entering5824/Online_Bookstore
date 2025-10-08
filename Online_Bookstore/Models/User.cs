using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Online_Bookstore.Models
{
[Table("Users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Column("FullName")]
    public string FullName { get; set; }

    [Required]
    [StringLength(100)]
    [Column("Email")]
    public string Email { get; set; }

    [Required]
    [StringLength(50)]
    [Column("Username")]
    public string Username { get; set; }

    [Required]
    [StringLength(255)]
    [Column("PasswordHash")]
    public string PasswordHash { get; set; }

    [Required]
    [StringLength(20)]
    [Column("Role")]
    public string Role { get; set; }

    [StringLength(20)]
    [Column("UserCode")]
    public string UserCode { get; set; }

    [StringLength(20)]
    [Column("PhoneNumber")]
    public string PhoneNumber { get; set; }

    [StringLength(255)]
    [Column("Address")]
    public string Address { get; set; }

    [Column("CreatedDate")]
    public DateTime CreatedDate { get; set; }

    [Column("UpdatedDate")]
    public DateTime UpdatedDate { get; set; }

    [Column("IsActive")]
    public bool IsActive { get; set; }

    [Column("LastLoginDate")]
    public DateTime? LastLoginDate { get; set; }
}}