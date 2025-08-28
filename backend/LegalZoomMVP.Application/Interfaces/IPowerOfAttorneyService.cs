using LegalZoomMVP.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LegalZoomMVP.Application.Interfaces
{
    public interface IPowerOfAttorneyService
    {
        Task<PowerOfAttorney> CreatePOAAsync(PowerOfAttorney poa);
        Task<PowerOfAttorney?> GetPOAByIdAsync(int id);
        Task<IEnumerable<PowerOfAttorney>> GetAllPOAsAsync();
        Task<PowerOfAttorney> UpdatePOAAsync(PowerOfAttorney poa);
        Task DeletePOAAsync(int id);
    }
}
