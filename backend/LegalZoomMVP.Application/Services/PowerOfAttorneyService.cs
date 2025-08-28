using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Domain.Entities;

namespace LegalZoomMVP.Application.Services
{
    public class PowerOfAttorneyService : IPowerOfAttorneyService
    {
        private readonly IPowerOfAttorneyRepository _repository;

        public PowerOfAttorneyService(IPowerOfAttorneyRepository repository)
        {
            _repository = repository;
        }

        public async Task<PowerOfAttorney> CreatePOAAsync(PowerOfAttorney poa)
        {
            return await _repository.AddAsync(poa);
        }

        public async Task<PowerOfAttorney?> GetPOAByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<PowerOfAttorney>> GetAllPOAsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PowerOfAttorney> UpdatePOAAsync(PowerOfAttorney poa)
        {
            return await _repository.UpdateAsync(poa);
        }

        public async Task DeletePOAAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
