using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Domain.Entities;
using LegalZoomMVP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LegalZoomMVP.Infrastructure.Services
{
    public class PowerOfAttorneyRepository(ApplicationDbContext context) : IPowerOfAttorneyRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<PowerOfAttorney> AddAsync(PowerOfAttorney poa)
        {
            _context.PowerOfAttorneys.Add(poa);
            await _context.SaveChangesAsync();

            return poa;
        }

        public async Task<PowerOfAttorney?> GetByIdAsync(int id)
        {
            return await _context.PowerOfAttorneys.FindAsync(id);
        }

        public async Task<IEnumerable<PowerOfAttorney>> GetAllAsync()
        {
            return await _context.PowerOfAttorneys.ToListAsync();
        }

        public async Task<PowerOfAttorney> UpdateAsync(PowerOfAttorney poa)
        {
            _context.PowerOfAttorneys.Update(poa);
            await _context.SaveChangesAsync();

            return poa;
        }

        public async Task DeleteAsync(int id)
        {
            var poa = await _context.PowerOfAttorneys.FindAsync(id);

            if (poa != null)
            {
                _context.PowerOfAttorneys.Remove(poa);

                await _context.SaveChangesAsync();
            }
        }
    }
}
