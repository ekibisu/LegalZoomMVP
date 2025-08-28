using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Domain.Entities;
using LegalZoomMVP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LegalZoomMVP.Infrastructure.Services
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByIdWithSubscriptionAsync(int id)
        {
            return await _context.Users.Include(u => u.Subscription).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetByEmailAndActiveAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);// && u.IsActive);

            return user;
        }
    }
}
