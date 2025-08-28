using LegalZoomMVP.Application.Interfaces;

namespace LegalZoomMVP.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        public Task<byte[]> GeneratePdfFromFormDataAsync(Dictionary<string, object> formData, string htmlTemplate)
        {
            // Stub implementation: return empty PDF
            return Task.FromResult(new byte[0]);
        }

        public Task<byte[]> GeneratePdfFromHtmlAsync(string htmlContent)
        {
            // Stub implementation: return empty PDF
            return Task.FromResult(new byte[0]);
        }
    }
}
