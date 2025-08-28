using System.Text.Json;
using LegalZoomMVP.Domain.Entities;
using LegalZoomMVP.Infrastructure.Data;
using LegalZoomMVP.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using LegalZoomMVP.Application.Interfaces;

namespace LegalZoomMVP.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardDto> GetDashboardDataAsync()
        {
            var totalUsers = await _context.Users.CountAsync(u => u.IsActive);
            var activeSubscriptions = await _context.Subscriptions.CountAsync(s => s.Status == SubscriptionStatus.Active);
            
            var currentMonth = DateTime.UtcNow.Date.AddDays(-DateTime.UtcNow.Day + 1);
            var monthlyRevenue = await _context.Payments
                .Where(p => p.Status == PaymentStatus.Completed && p.CompletedAt >= currentMonth)
                .SumAsync(p => p.Amount);

            var totalRevenue = await _context.Payments
                .Where(p => p.Status == PaymentStatus.Completed)
                .SumAsync(p => p.Amount);

            var formsCreated = await _context.UserForms.CountAsync();

            // Revenue by month (last 12 months)
            var revenueByMonth = await GetRevenueByMonth();
            
            // Popular forms
            var popularForms = await GetPopularForms();

            return new AdminDashboardDto
            {
                TotalUsers = totalUsers,
                ActiveSubscriptions = activeSubscriptions,
                MonthlyRevenue = monthlyRevenue,
                TotalRevenue = totalRevenue,
                FormsCreated = formsCreated,
                RevenueByMonth = revenueByMonth,
                PopularForms = popularForms
            };
        }

        public async Task<IEnumerable<UserManagementDto>> GetUsersAsync(int page, int pageSize)
        {
            var users = await _context.Users
                .Include(u => u.Subscription)
                .Include(u => u.Payments)
                .Include(u => u.UserForms)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return users.Select(u => new UserManagementDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role.ToString(),
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                HasActiveSubscription = u.Subscription?.Status == SubscriptionStatus.Active,
                TotalSpent = u.Payments.Where(p => p.Status == PaymentStatus.Completed).Sum(p => p.Amount),
                FormsCompleted = u.UserForms.Count(uf => uf.Status == FormStatus.Completed)
            });
        }

        public async Task<bool> UpdateUserStatusAsync(int userId, bool isActive)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsActive = isActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<FormTemplateDto> CreateFormTemplateAsync(CreateFormTemplateDto request)
        {
            var template = new FormTemplate
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                Price = request.Price,
                IsPremium = request.IsPremium,
                FormSchema = JsonSerializer.Serialize(request.FormSchema),
                HtmlTemplate = request.HtmlTemplate,
                CreatedByUserId = 1, // TODO: Get from current admin user context
                CreatedAt = DateTime.UtcNow
            };

            _context.FormTemplates.Add(template);
            await _context.SaveChangesAsync();

            return new FormTemplateDto
            {
                Id = template.Id,
                Name = template.Name,
                Description = template.Description,
                Category = template.Category,
                Price = template.Price,
                IsPremium = template.IsPremium,
                FormSchema = JsonSerializer.Deserialize<FormSchemaDto>(template.FormSchema)!
            };
        }

        public async Task<FormTemplateDto?> GetFormTemplateAsync(int id)
        {
            var template = await _context.FormTemplates
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (template == null) return null;

            return new FormTemplateDto
            {
                Id = template.Id,
                Name = template.Name,
                Description = template.Description,
                Category = template.Category,
                Price = template.Price,
                IsPremium = template.IsPremium,
                FormSchema = JsonSerializer.Deserialize<FormSchemaDto>(template.FormSchema)!
            };
        }

        public async Task<IEnumerable<FormTemplateDto>> GetAllFormTemplatesAsync(bool includeInactive)
        {
            var query = _context.FormTemplates.AsQueryable();
            
            if (!includeInactive)
                query = query.Where(t => t.IsActive);

            var templates = await query.OrderBy(t => t.Category).ThenBy(t => t.Name).ToListAsync();

            return templates.Select(t => new FormTemplateDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Category = t.Category,
                Price = t.Price,
                IsPremium = t.IsPremium,
                FormSchema = JsonSerializer.Deserialize<FormSchemaDto>(t.FormSchema)!
            });
        }

        public async Task<FormTemplateDto?> UpdateFormTemplateAsync(int id, UpdateFormTemplateDto request)
        {
            var template = await _context.FormTemplates.FindAsync(id);
            if (template == null) return null;

            if (request.Name != null) template.Name = request.Name;
            if (request.Description != null) template.Description = request.Description;
            if (request.Category != null) template.Category = request.Category;
            if (request.Price.HasValue) template.Price = request.Price.Value;
            if (request.IsPremium.HasValue) template.IsPremium = request.IsPremium.Value;
            if (request.FormSchema != null) template.FormSchema = JsonSerializer.Serialize(request.FormSchema);
            if (request.HtmlTemplate != null) template.HtmlTemplate = request.HtmlTemplate;
            if (request.IsActive.HasValue) template.IsActive = request.IsActive.Value;

            template.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new FormTemplateDto
            {
                Id = template.Id,
                Name = template.Name,
                Description = template.Description,
                Category = template.Category,
                Price = template.Price,
                IsPremium = template.IsPremium,
                FormSchema = JsonSerializer.Deserialize<FormSchemaDto>(template.FormSchema)!
            };
        }

        public async Task<bool> DeleteFormTemplateAsync(int id)
        {
            var template = await _context.FormTemplates.FindAsync(id);
            if (template == null) return false;

            // Check if template is in use
            var isInUse = await _context.UserForms.AnyAsync(uf => uf.FormTemplateId == id);
            if (isInUse)
                throw new InvalidOperationException("Cannot delete form template that is in use");

            _context.FormTemplates.Remove(template);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<List<RevenueByMonthDto>> GetRevenueByMonth()
        {
            var twelveMonthsAgo = DateTime.UtcNow.AddMonths(-12);
            
            var revenueData = await _context.Payments
                .Where(p => p.Status == PaymentStatus.Completed && p.CompletedAt >= twelveMonthsAgo)
                .GroupBy(p => new { Month = p.CompletedAt.Value.Month, Year = p.CompletedAt.Value.Year })
                .Select(g => new { g.Key.Month, g.Key.Year, Revenue = g.Sum(p => p.Amount) })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            var result = new List<RevenueByMonthDto>();
            var currentDate = twelveMonthsAgo;

            while (currentDate <= DateTime.UtcNow)
            {
                var monthData = revenueData.FirstOrDefault(x => x.Month == currentDate.Month && x.Year == currentDate.Year);
                result.Add(new RevenueByMonthDto
                {
                    Month = currentDate.ToString("MMMM yyyy"),
                    Amount = monthData?.Revenue ?? 0
                });
                currentDate = currentDate.AddMonths(1);
            }

            return result;
        }

        private async Task<List<PopularFormDto>> GetPopularForms()
        {
            return await _context.FormTemplates
                .Include(t => t.UserForms)
                .Select(t => new PopularFormDto
                {
                    FormName = t.Name,
                    UsageCount = t.UserForms.Count,
                    Revenue = _context.Payments
                        .Where(p => p.Status == PaymentStatus.Completed && p.FormTemplateId == t.Id)
                        .Sum(p => p.Amount)
                })
                .OrderByDescending(f => f.UsageCount)
                .Take(10)
                .ToListAsync();
        }
    }
}