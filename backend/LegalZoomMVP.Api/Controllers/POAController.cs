using LegalZoomMVP.Application.DTOs;
using LegalZoomMVP.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LegalZoomMVP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class POAController(PowerOfAttorneyService service) : ControllerBase
    {
        private readonly PowerOfAttorneyService _service = service;

        [HttpPost]
        public async Task<IActionResult> SubmitPOA([FromBody] PowerOfAttorneyDto dto)
        {
            var poa = new LegalZoomMVP.Domain.Entities.PowerOfAttorney
            {
                // Section 1
                StateOfResidence = dto.StateOfResidence,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                MobileNumber = dto.MobileNumber,
                IsMarried = dto.IsMarried,
                StreetAddress = dto.StreetAddress,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,

                // Section 2
                AgentFirstName = dto.AgentFirstName,
                AgentLastName = dto.AgentLastName,
                AgentCity = dto.AgentCity,
                AgentState = dto.AgentState,
                AgentZipCode = dto.AgentZipCode,
                AlternateAgentName = dto.AlternateAgentName,

                // Section 3
                RealEstate = dto.RealEstate,
                PersonalProperty = dto.PersonalProperty,
                Banking = dto.Banking,
                Stocks = dto.Stocks,
                BusinessOperations = dto.BusinessOperations,
                RetirementPlans = dto.RetirementPlans,
                Insurance = dto.Insurance,
                EstateTrusts = dto.EstateTrusts,
                GovernmentAssistance = dto.GovernmentAssistance,
                PersonalFamilyCare = dto.PersonalFamilyCare,
                MakingGifts = dto.MakingGifts,
                PetCare = dto.PetCare,

                // Section 4
                EffectiveDate = dto.EffectiveDate,
                RevokePrior = dto.RevokePrior,
                AdditionalInstructions = dto.AdditionalInstructions
            };
            var result = await _service.CreatePOAAsync(poa);
            return Ok(new { message = "POA form submitted successfully", id = result.Id });
        }
    }
}
