namespace LegalZoomMVP.Application.Interfaces
{
    public interface IPaymentRepository
    {
        // Example method signatures based on PaymentService usage
        Task<LegalZoomMVP.Domain.Entities.User?> GetUserByIdAsync(int userId);
        Task<LegalZoomMVP.Domain.Entities.FormTemplate?> GetFormTemplateByIdAsync(int formTemplateId);
        Task<IEnumerable<LegalZoomMVP.Domain.Entities.Payment>> GetPaymentsByUserIdAsync(int userId);
    }
}
