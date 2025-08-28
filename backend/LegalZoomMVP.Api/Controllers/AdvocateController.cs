using Microsoft.AspNetCore.Mvc;
using LegalZoomMVP.Application.DTOs;
using LegalZoomMVP.Application.Interfaces;

namespace LegalZoomMVP.Api.Controllers
{
    [ApiController]
    [Route("api/advocate")]
    public class AdvocateController : ControllerBase
    {
        private readonly IAdvocateService _advocateService;

        public AdvocateController(IAdvocateService advocateService)
        {
            _advocateService = advocateService;
        }

        // GET: api/advocate/profile?email=...
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile([FromQuery] string email)
        {
            var profile = await _advocateService.GetProfileAsync(email);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        // PUT: api/advocate/profile
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] AdvocateDto dto)
        {
            var result = await _advocateService.UpdateProfileAsync(dto);
            if (!result) return BadRequest();
            return Ok(new { message = "Advocate profile updated" });
        }

        // POST: api/advocate/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AdvocateDto dto)
        {
            var result = await _advocateService.UpdateProfileAsync(dto);
            if (!result) return BadRequest("Failed to register advocate");
            return Ok(new { message = "Advocate registered" });
        }
    }
}
