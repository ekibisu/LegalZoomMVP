using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Domain.Entities;
using LegalZoomMVP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LegalZoomMVP.Infrastructure.Services
{
    public class FormRepository(ApplicationDbContext context) : IFormRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<UserForm> CreateUserFormAsync(UserForm userForm)
        {
            _context.UserForms.Add(userForm);
            await _context.SaveChangesAsync();
            return userForm;
        }

        public async Task<IEnumerable<FormTemplate>> GetActiveFormTemplatesAsync()
        {
            return await _context.FormTemplates.Where(f => f.IsActive).ToListAsync();
        }

        public async Task<FormTemplate?> GetFormTemplateByIdAsync(int id)
        {
            return await _context.FormTemplates.FindAsync(id);
        }

        public async Task<UserForm?> GetUserFormByIdAsync(int id)
        {
            return await _context.UserForms
                .Include(uf => uf.FormTemplate)
                .FirstOrDefaultAsync(uf => uf.Id == id);
        }

        public async Task<IEnumerable<UserForm>> GetUserFormsByUserIdAsync(int userId)
        {
            return await _context.UserForms.Where(uf => uf.UserId == userId).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserFormAsync(UserForm userForm)
        {
            _context.UserForms.Update(userForm);
            await _context.SaveChangesAsync();
        }

        // ...existing code...
    }
}
