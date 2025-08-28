using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Domain.Entities;
using LegalZoomMVP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LegalZoomMVP.Infrastructure.Services
{
    public class AdvocateRepository : IAdvocateRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AdvocateRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddAdvocateAsync(Advocate advocate)
        {
            _dbContext.Advocates.Add(advocate);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Advocate> GetAdvocateByEmailAsync(string email)
        {
            return await _dbContext.Advocates.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<bool> UpdateAdvocateAsync(Advocate advocate)
        {
            _dbContext.Advocates.Update(advocate);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
