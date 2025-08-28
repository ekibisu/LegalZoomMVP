using LegalZoomMVP.Application.DTOs;
using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace LegalZoomMVP.Application.Services
{
public class AdvocateService(IAdvocateRepository advocateRepository) : IAdvocateService
{
    private readonly IAdvocateRepository _advocateRepository = advocateRepository;

        private static string HashPassword(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = SHA256.HashData(bytes);

            return Convert.ToBase64String(hash);
        }

        public async Task<bool> CreateAdvocateAsync(AdvocateDto dto)
        {
            var passwordHash = HashPassword(dto.Password);

            var advocate = new LegalZoomMVP.Domain.Entities.Advocate
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                NationalId = dto.NationalId,
                PassportNumber = dto.PassportNumber,
                Gender = dto.Gender,
                LskP105 = dto.LskP105,
                MobileNumber = dto.MobileNumber,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = UserRole.Lawyer
            };
            return await _advocateRepository.AddAdvocateAsync(advocate);
        }

        public async Task<AdvocateDto> GetProfileAsync(string email)
        {
            var advocate = await _advocateRepository.GetAdvocateByEmailAsync(email);

            if (advocate == null) return null;

            return new AdvocateDto
            {
                FirstName = advocate.FirstName,
                LastName = advocate.LastName,
                NationalId = advocate.NationalId,
                PassportNumber = advocate.PassportNumber,
                Gender = advocate.Gender,
                LskP105 = advocate.LskP105,
                MobileNumber = advocate.MobileNumber,
                Email = advocate.Email,
                Password = advocate.PasswordHash ?? string.Empty,
                Role = Enum.IsDefined(typeof(LegalZoomMVP.Domain.Entities.UserRole), advocate.Role) ? (LegalZoomMVP.Domain.Entities.UserRole)advocate.Role : LegalZoomMVP.Domain.Entities.UserRole.Client
            };
        }

        public async Task<bool> UpdateProfileAsync(AdvocateDto dto)
        {
            var advocate = await _advocateRepository.GetAdvocateByEmailAsync(dto.Email);

            if (advocate == null) return false;

            advocate.FirstName = dto.FirstName;
            advocate.LastName = dto.LastName;
            advocate.NationalId = dto.NationalId;
            advocate.PassportNumber = dto.PassportNumber;
            advocate.Gender = dto.Gender;
            advocate.LskP105 = dto.LskP105;
            advocate.MobileNumber = dto.MobileNumber;
            advocate.PasswordHash = dto.Password; // Hash before storing in production
            advocate.Role = UserRole.Lawyer;
            advocate.UpdatedAt = DateTime.UtcNow;
            return await _advocateRepository.UpdateAdvocateAsync(advocate);
        }
    }
}
