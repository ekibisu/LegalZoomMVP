using LegalZoomMVP.Application.DTOs;
using LegalZoomMVP.Application.Exceptions;
using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LegalZoomMVP.Application.Services
{
    public class AuthService(IUserRepository userRepository, IJwtConfiguration jwtConfiguration) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtConfiguration _jwtConfiguration = jwtConfiguration;

        private static string HashPassword(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = SHA256.HashData(bytes);

            return Convert.ToBase64String(hash);
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);

            if (string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            return hash == storedHash;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto request)
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var passwordHash = HashPassword(request.Password);

            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = UserRole.Client
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role.ToString()
                }
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.GetByEmailAndActiveAsync(request.Email) ?? throw new UnauthorizedAccessException("Invalid email or password");

            if (!VerifyPassword(request.Password.Trim(), user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("User account is inactive");
            }

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role.ToString()
                }
            };
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfiguration.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirationMinutes),
                Issuer = _jwtConfiguration.Issuer,
                Audience = _jwtConfiguration.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<UserDto> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdWithSubscriptionAsync(userId);

            return user == null
                ? throw new NotFoundException("User not found")
                : new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                HasActiveSubscription = user.Subscription?.Status == SubscriptionStatus.Active
            };
        }

        public async Task<UserDto> UpdateUserProfileAsync(int userId, UpdateUserDto request)
        {
            var user = await _userRepository.GetByIdAsync(userId) ?? throw new NotFoundException("User not found");

            if (!string.IsNullOrEmpty(request.FirstName))
                user.FirstName = request.FirstName;

            if (!string.IsNullOrEmpty(request.LastName))
                user.LastName = request.LastName;

            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                HasActiveSubscription = user.Subscription?.Status == SubscriptionStatus.Active
            };
        }
    }
}