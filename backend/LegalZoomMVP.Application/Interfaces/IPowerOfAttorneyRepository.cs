using LegalZoomMVP.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LegalZoomMVP.Application.Interfaces
{
    public interface IPowerOfAttorneyRepository
    {
        Task<PowerOfAttorney> AddAsync(PowerOfAttorney poa);
        Task<PowerOfAttorney?> GetByIdAsync(int id);
        Task<IEnumerable<PowerOfAttorney>> GetAllAsync();
        Task<PowerOfAttorney> UpdateAsync(PowerOfAttorney poa);
        Task DeleteAsync(int id);
    }
}
