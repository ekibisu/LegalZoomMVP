namespace LegalZoomMVP.Application.DTOs
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }
        public int ActiveSubscriptions { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
        public int FormsCreated { get; set; }
        public List<RevenueByMonthDto> RevenueByMonth { get; set; } = new();
        public List<PopularFormDto> PopularForms { get; set; } = new();
    }
}