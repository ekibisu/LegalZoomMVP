namespace LegalZoomMVP.Application.DTOs
{
    public class PopularFormDto
    {
        public string FormName { get; set; } = string.Empty;
        public int UsageCount { get; set; }
        public decimal Revenue { get; set; }
    }
}