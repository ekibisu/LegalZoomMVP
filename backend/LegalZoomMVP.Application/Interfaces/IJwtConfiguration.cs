namespace LegalZoomMVP.Application.Interfaces
{
    public interface IJwtConfiguration
    {
        string SecretKey { get; }
        string Issuer { get; }
        string Audience { get; }
        int ExpirationMinutes { get; }
    }
} 