using LegalZoomMVP.Application.DTOs;

namespace LegalZoomMVP.Application.Interfaces
{
    public interface IFormService
    {
        Task<IEnumerable<FormTemplateDto>> GetFormTemplatesAsync();
        Task<FormTemplateDto?> GetFormTemplateAsync(int id);
        Task<UserFormDto> CreateUserFormAsync(int userId, CreateUserFormDto request);
        Task<IEnumerable<UserFormDto>> GetUserFormsAsync(int userId);
        Task<UserFormDto?> GetUserFormAsync(int formId);
        Task<UserFormDto> UpdateUserFormAsync(int formId, UpdateUserFormDto request);
        Task<byte[]> ExportFormToPdfAsync(int formId);
        Task<byte[]> ExportFormToPdfFromHtmlAsync(string htmlContent);
    }
}