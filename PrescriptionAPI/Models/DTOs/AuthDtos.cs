using System.ComponentModel.DataAnnotations;

namespace PrescriptionAPI.Models.DTOs;

public class RegisterDto
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }
        
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }
}

public class LoginDto
{
    [Required]
    public string Username { get; set; }
        
    [Required]
    public string Password { get; set; }
}

public class TokenResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}

public class RefreshTokenDto
{
    [Required]
    public string RefreshToken { get; set; }
}