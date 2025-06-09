using System.ComponentModel.DataAnnotations;

namespace PrescriptionAPI.Models.Entities;

public class RefreshToken
{
    public int Id { get; set; }
        
    [Required]
    public string Token { get; set; }
        
    public DateTime ExpiryDate { get; set; }
        
    public bool IsRevoked { get; set; }
        
    public int IdUser { get; set; }
    public virtual User User { get; set; }
        
    public DateTime CreatedAt { get; set; }
}