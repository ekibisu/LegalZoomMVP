using LegalZoomMVP.Domain.Entities;
using System.Threading.Tasks;

namespace LegalZoomMVP.Application.Interfaces
{
    public interface IAdvocateRepository
    {
        Task<bool> AddAdvocateAsync(Advocate advocate);
        Task<Advocate> GetAdvocateByEmailAsync(string email);
        Task<bool> UpdateAdvocateAsync(Advocate advocate);
    }
}
