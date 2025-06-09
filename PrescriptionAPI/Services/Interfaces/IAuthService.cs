using PrescriptionAPI.Models.DTOs;

namespace PrescriptionAPI.Services.Interfaces;

public interface IAuthService
{
    Task<TokenResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<TokenResponseDto> LoginAsync(LoginDto loginDto);
    Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
    Task RevokeTokenAsync(string refreshToken);
}