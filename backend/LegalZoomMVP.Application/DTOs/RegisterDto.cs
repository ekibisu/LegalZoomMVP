using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Application.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        // User type: "customer" or "advocate"
        [Required]
        public string UserType { get; set; } = "customer";

        // Common fields
        public string NationalId { get; set; } = string.Empty;
        public string PassportNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        // Advocate-only
        public string LskP105 { get; set; } = string.Empty;

        // Role for registration (Client/Lawyer)
        public LegalZoomMVP.Domain.Entities.UserRole Role { get; set; } = LegalZoomMVP.Domain.Entities.UserRole.Client;
    }
}
