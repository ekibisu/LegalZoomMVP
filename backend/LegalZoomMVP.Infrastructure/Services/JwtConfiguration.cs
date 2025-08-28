using LegalZoomMVP.Application.Interfaces;

namespace LegalZoomMVP.Infrastructure.Services
{
    public class JwtConfiguration : IJwtConfiguration
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; } = 60;
    }
}
