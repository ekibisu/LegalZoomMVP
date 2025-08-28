using LegalZoomMVP.Application.DTOs;
using LegalZoomMVP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LegalZoomMVP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAdvocateService advocateService, IAuthService authService) : ControllerBase
    {
        private readonly IAdvocateService _advocateService = advocateService;
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var userType = dto.UserType?.ToLower();
                var passwordHash = HashPassword(dto.Password);

                if (userType == "advocate" || userType == "lawyer")
                {
                    var advocateDto = new AdvocateDto
                    {
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        NationalId = dto.NationalId,
                        PassportNumber = dto.PassportNumber,
                        Gender = dto.Gender,
                        LskP105 = dto.LskP105,
                        MobileNumber = dto.MobileNumber,
                        Email = dto.Email,
                        Password = dto.Password,
                        Role = LegalZoomMVP.Domain.Entities.UserRole.Lawyer
                    };

                    var result = await _advocateService.CreateAdvocateAsync(advocateDto);

                    if (!result)
                    {
                        return BadRequest("Failed to register advocate");
                    }

                    // Also create a User entry for advocate
                    var user = new LegalZoomMVP.Domain.Entities.User
                    {
                        Email = dto.Email,
                        PasswordHash = passwordHash,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Role = LegalZoomMVP.Domain.Entities.UserRole.Lawyer
                    };
                    // Get IUserRepository from DI
                    var userRepository = HttpContext.RequestServices.GetService(typeof(LegalZoomMVP.Application.Interfaces.IUserRepository)) as LegalZoomMVP.Application.Interfaces.IUserRepository;
                    
                    if (userRepository != null)
                    {
                        await userRepository.AddAsync(user);
                        await userRepository.SaveChangesAsync();
                    }

                    return Ok(new { message = "Advocate registered" });
                }
                // Customer registration logic
                // Use RegisterAsync from IAuthService, passing RegisterDto
                dto.Role = LegalZoomMVP.Domain.Entities.UserRole.Client;
                var userResult = await _authService.RegisterAsync(dto);
                return Ok(userResult);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _authService.GetUserProfileAsync(userId);
            return Ok(user);
        }

        private static string HashPassword(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = SHA256.HashData(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}