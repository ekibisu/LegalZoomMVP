using LegalZoomMVP.Application.DTOs;
using System.Threading.Tasks;

namespace LegalZoomMVP.Application.Interfaces
{
    public interface IAdvocateService
    {
        Task<AdvocateDto> GetProfileAsync(string email);
        Task<bool> UpdateProfileAsync(AdvocateDto dto);
        Task<bool> CreateAdvocateAsync(AdvocateDto dto);
    }
}
