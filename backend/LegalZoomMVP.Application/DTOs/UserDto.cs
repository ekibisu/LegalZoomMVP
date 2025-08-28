namespace LegalZoomMVP.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool HasActiveSubscription { get; set; }
        public string NationalId { get; set; } = string.Empty;
        public string PassportNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}