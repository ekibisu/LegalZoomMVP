using LegalZoomMVP.Domain.Entities;

namespace LegalZoomMVP.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByEmailAndActiveAsync(string email);
        Task<User?> GetByIdWithSubscriptionAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task SaveChangesAsync();
    }
} 