using System;
using System.ComponentModel.DataAnnotations;

public class OtpRecord
{
    [Key]
    public int Id { get; set; }  // Add primary key for EF

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(6)]
    public string OtpCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ExpireAt { get; set; }
}