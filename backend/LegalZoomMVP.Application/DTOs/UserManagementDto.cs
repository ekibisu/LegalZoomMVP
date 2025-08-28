namespace LegalZoomMVP.Application.DTOs
{
    public class UserManagementDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool HasActiveSubscription { get; set; }
        public decimal TotalSpent { get; set; }
        public int FormsCompleted { get; set; }
    }
}