using LegalZoomMVP.Application.DTOs;

namespace LegalZoomMVP.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto request);
        Task<AuthResponseDto> LoginAsync(LoginDto request);
        Task<UserDto> GetUserProfileAsync(int userId);
        Task<UserDto> UpdateUserProfileAsync(int userId, UpdateUserDto request);
    }
}