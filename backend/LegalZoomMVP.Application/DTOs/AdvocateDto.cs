namespace LegalZoomMVP.Application.DTOs
{
    public class AdvocateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public string PassportNumber { get; set; }
        public string Gender { get; set; }
        public string LskP105 { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public LegalZoomMVP.Domain.Entities.UserRole Role { get; set; } = LegalZoomMVP.Domain.Entities.UserRole.Client;
    }
}
