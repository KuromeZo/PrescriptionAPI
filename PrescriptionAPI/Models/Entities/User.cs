using System.ComponentModel.DataAnnotations;

namespace PrescriptionAPI.Models.Entities;

public class User
{
    public int IdUser { get; set; }
        
    [Required]
    [StringLength(50)]
    public string Username { get; set; }
        
    [Required]
    public string PasswordHash { get; set; }
        
    [Required]
    public string Salt { get; set; }
        
    public DateTime CreatedAt { get; set; }
        
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}