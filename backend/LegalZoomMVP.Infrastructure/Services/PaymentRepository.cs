using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Domain.Entities;
using LegalZoomMVP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LegalZoomMVP.Infrastructure.Services
{
    public class PaymentRepository(ApplicationDbContext context) : IPaymentRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<FormTemplate?> GetFormTemplateByIdAsync(int formTemplateId)
        {
            return await _context.FormTemplates.FindAsync(formTemplateId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(int userId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId)
                .Include(p => p.FormTemplate)
                .Include(p => p.Subscription)
                .ToListAsync();
        }
    }
}
