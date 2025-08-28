using LegalZoomMVP.Domain.Entities;

namespace LegalZoomMVP.Application.Interfaces
{
    public interface IFormRepository
    {
        Task<IEnumerable<FormTemplate>> GetActiveFormTemplatesAsync();
        Task<FormTemplate?> GetFormTemplateByIdAsync(int id);
        Task<UserForm> CreateUserFormAsync(UserForm userForm);
        Task<IEnumerable<UserForm>> GetUserFormsByUserIdAsync(int userId);
        Task<UserForm?> GetUserFormByIdAsync(int id);
        Task UpdateUserFormAsync(UserForm userForm);
        Task SaveChangesAsync();
    }
} 