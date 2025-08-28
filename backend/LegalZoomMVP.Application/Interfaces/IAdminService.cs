using LegalZoomMVP.Application.DTOs;

namespace LegalZoomMVP.Application.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardDto> GetDashboardDataAsync();
        Task<IEnumerable<UserManagementDto>> GetUsersAsync(int page, int pageSize);
        Task<bool> UpdateUserStatusAsync(int userId, bool isActive);
        Task<FormTemplateDto> CreateFormTemplateAsync(CreateFormTemplateDto request);
        Task<FormTemplateDto?> GetFormTemplateAsync(int id);
        Task<FormTemplateDto?> UpdateFormTemplateAsync(int id, UpdateFormTemplateDto request);
        Task<bool> DeleteFormTemplateAsync(int id);
        Task<IEnumerable<FormTemplateDto>> GetAllFormTemplatesAsync(bool includeInactive);
    }
}