namespace LegalZoomMVP.Application.Interfaces
{
    public interface IPdfService
    {
        Task<byte[]> GeneratePdfFromFormDataAsync(Dictionary<string, object> formData, string htmlTemplate);
        Task<byte[]> GeneratePdfFromHtmlAsync(string htmlContent);
    }
}
