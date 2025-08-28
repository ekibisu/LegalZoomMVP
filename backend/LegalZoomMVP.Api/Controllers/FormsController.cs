using LegalZoomMVP.Application.DTOs;
using LegalZoomMVP.Application.Exceptions;
using LegalZoomMVP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LegalZoomMVP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FormsController(IFormService formService) : ControllerBase
    {
        private readonly IFormService _formService = formService;

        [HttpGet("templates")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<FormTemplateDto>>> GetFormTemplates()
        {
            var templates = await _formService.GetFormTemplatesAsync();

            return Ok(templates);
        }

        [HttpGet("templates/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<FormTemplateDto>> GetFormTemplate(int id)
        {
            var template = await _formService.GetFormTemplateAsync(id);

            if (template == null)
                return NotFound(new { message = "Form template not found" });

            return Ok(template);
        }

        [HttpPost("user-forms")]
        public async Task<ActionResult<UserFormDto>> CreateUserForm(CreateUserFormDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            try
            {
                var userForm = await _formService.CreateUserFormAsync(userId, request);
                return CreatedAtAction(nameof(GetUserForm), new { id = userForm.Id }, userForm);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("user-forms")]
        public async Task<ActionResult<IEnumerable<UserFormDto>>> GetUserForms()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var userForms = await _formService.GetUserFormsAsync(userId);

            return Ok(userForms);
        }

        [HttpGet("user-forms/{id}")]
        public async Task<ActionResult<UserFormDto>> GetUserForm(int id)
        {
            _ = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var userForm = await _formService.GetUserFormAsync(id);
            
            if (userForm == null)
                return NotFound(new { message = "User form not found" });

            return Ok(userForm);
        }

        [HttpPut("user-forms/{id}")]
        public async Task<ActionResult<UserFormDto>> UpdateUserForm(int id, UpdateUserFormDto request)
        {
            _ = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var userForm = await _formService.UpdateUserFormAsync(id, request);

                return Ok(userForm);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("user-forms/{id}/export")]
        public async Task<IActionResult> ExportFormToPdf(int id)
        {
            _ = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var pdfBytes = await _formService.ExportFormToPdfAsync(id);
                return File(pdfBytes, "application/pdf", $"form_{id}.pdf");
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}