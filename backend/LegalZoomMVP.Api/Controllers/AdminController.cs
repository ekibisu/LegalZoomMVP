using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LegalZoomMVP.Application.DTOs;
using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Application.Exceptions;

namespace LegalZoomMVP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController(IAdminService adminService) : ControllerBase
    {
        private readonly IAdminService _adminService = adminService;

        [HttpGet("dashboard")]
        public async Task<ActionResult<AdminDashboardDto>> GetDashboard()
        {
            var dashboard = await _adminService.GetDashboardDataAsync();
            return Ok(dashboard);
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserManagementDto>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var users = await _adminService.GetUsersAsync(page, pageSize);
            return Ok(users);
        }

        [HttpPut("users/{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] bool isActive)
        {
            var success = await _adminService.UpdateUserStatusAsync(id, isActive);
            
            if (!success)
                return NotFound(new { message = "User not found" });

            return Ok(new { message = "User status updated successfully" });
        }

        [HttpPost("form-templates")]
        public async Task<ActionResult<FormTemplateDto>> CreateFormTemplate(CreateFormTemplateDto request)
        {
            try
            {
                var template = await _adminService.CreateFormTemplateAsync(request);
                return CreatedAtAction(nameof(GetFormTemplate), new { id = template.Id }, template);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("form-templates/{id}")]
        public async Task<ActionResult<FormTemplateDto>> GetFormTemplate(int id)
        {
            var template = await _adminService.GetFormTemplateAsync(id);
            
            if (template == null)
                return NotFound(new { message = "Form template not found" });

            return Ok(template);
        }

        [HttpPut("form-templates/{id}")]
        public async Task<ActionResult<FormTemplateDto>> UpdateFormTemplate(int id, UpdateFormTemplateDto request)
        {
            try
            {
                var template = await _adminService.UpdateFormTemplateAsync(id, request);
                if (template == null)
                    return NotFound(new { message = "Form template not found" });

                return Ok(template);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("form-templates/{id}")]
        public async Task<IActionResult> DeleteFormTemplate(int id)
        {
            var success = await _adminService.DeleteFormTemplateAsync(id);
            
            if (!success)
                return NotFound(new { message = "Form template not found" });

            return NoContent();
        }

        [HttpGet("form-templates")]
        public async Task<ActionResult<IEnumerable<FormTemplateDto>>> GetAllFormTemplates([FromQuery] bool includeInactive = false)
        {
            var templates = await _adminService.GetAllFormTemplatesAsync(includeInactive);
            return Ok(templates);
        }
    }
}