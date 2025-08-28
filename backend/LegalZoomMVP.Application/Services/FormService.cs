using System.Text.Json;
using System.Text.RegularExpressions;
using LegalZoomMVP.Application.DTOs;
using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Application.Exceptions;
using LegalZoomMVP.Domain.Entities;

namespace LegalZoomMVP.Application.Services
{
    public class FormService(IFormRepository formRepository, IPdfService pdfService) : IFormService
    {
        private readonly IFormRepository _formRepository = formRepository;
        private readonly IPdfService _pdfService = pdfService;

        public async Task<IEnumerable<FormTemplateDto>> GetFormTemplatesAsync()
        {
            var templates = await _formRepository.GetActiveFormTemplatesAsync();

            return templates.Select(t => new FormTemplateDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Category = t.Category,
                Price = t.Price,
                IsPremium = t.IsPremium,
                FormSchema = JsonSerializer.Deserialize<FormSchemaDto>(t.FormSchema)!
            });
        }

        public async Task<FormTemplateDto?> GetFormTemplateAsync(int id)
        {
            var template = await _formRepository.GetFormTemplateByIdAsync(id);

            if (template == null) return null;

            return new FormTemplateDto
            {
                Id = template.Id,
                Name = template.Name,
                Description = template.Description,
                Category = template.Category,
                Price = template.Price,
                IsPremium = template.IsPremium,
                FormSchema = JsonSerializer.Deserialize<FormSchemaDto>(template.FormSchema)!
            };
        }

        public async Task<UserFormDto> CreateUserFormAsync(int userId, CreateUserFormDto request)
        {
            var template = await _formRepository.GetFormTemplateByIdAsync(request.FormTemplateId);
            if (template == null)
                throw new NotFoundException("Form template not found");

            var userForm = new UserForm
            {
                UserId = userId,
                FormTemplateId = request.FormTemplateId,
                FormData = JsonSerializer.Serialize(request.FormData),
                Status = FormStatus.Draft
            };

            await _formRepository.CreateUserFormAsync(userForm);
            await _formRepository.SaveChangesAsync();

            return new UserFormDto
            {
                Id = userForm.Id,
                FormTemplateName = template.Name,
                Status = userForm.Status.ToString(),
                CreatedAt = userForm.CreatedAt,
                FormData = JsonSerializer.Deserialize<Dictionary<string, object>>(userForm.FormData)!
            };
        }

        public async Task<IEnumerable<UserFormDto>> GetUserFormsAsync(int userId)
        {
            var userForms = await _formRepository.GetUserFormsByUserIdAsync(userId);

            return userForms.Select(uf => new UserFormDto
            {
                Id = uf.Id,
                FormTemplateName = uf.FormTemplate.Name,
                Status = uf.Status.ToString(),
                CreatedAt = uf.CreatedAt,
                CompletedAt = uf.CompletedAt,
                FormData = JsonSerializer.Deserialize<Dictionary<string, object>>(uf.FormData)!
            });
        }

        public async Task<UserFormDto?> GetUserFormAsync(int id)
        {
            var userForm = await _formRepository.GetUserFormByIdAsync(id);
            if (userForm == null) return null;

            return new UserFormDto
            {
                Id = userForm.Id,
                FormTemplateName = userForm.FormTemplate.Name,
                Status = userForm.Status.ToString(),
                CreatedAt = userForm.CreatedAt,
                CompletedAt = userForm.CompletedAt,
                FormData = JsonSerializer.Deserialize<Dictionary<string, object>>(userForm.FormData)!
            };
        }

        public async Task<UserFormDto> UpdateUserFormAsync(int id, UpdateUserFormDto request)
        {
            var userForm = await _formRepository.GetUserFormByIdAsync(id);
            if (userForm == null)
                throw new NotFoundException("User form not found");

            userForm.FormData = JsonSerializer.Serialize(request.FormData);
            if (request.IsCompleted)
            {
                userForm.Status = FormStatus.Completed;
                userForm.CompletedAt = DateTime.UtcNow;
            }
            userForm.UpdatedAt = DateTime.UtcNow;

            await _formRepository.UpdateUserFormAsync(userForm);
            await _formRepository.SaveChangesAsync();

            return new UserFormDto
            {
                Id = userForm.Id,
                FormTemplateName = userForm.FormTemplate.Name,
                Status = userForm.Status.ToString(),
                CreatedAt = userForm.CreatedAt,
                CompletedAt = userForm.CompletedAt,
                FormData = JsonSerializer.Deserialize<Dictionary<string, object>>(userForm.FormData)!
            };
        }

        public async Task<byte[]> ExportFormToPdfAsync(int userFormId)
        {
            var userForm = await _formRepository.GetUserFormByIdAsync(userFormId);
            if (userForm == null)
                throw new NotFoundException("User form not found");

            var formData = JsonSerializer.Deserialize<Dictionary<string, object>>(userForm.FormData)!;
            var htmlTemplate = userForm.FormTemplate.HtmlTemplate;

            return await _pdfService.GeneratePdfFromFormDataAsync(formData, htmlTemplate);
        }

        public async Task<byte[]> ExportFormToPdfFromHtmlAsync(string htmlContent)
        {
            return await _pdfService.GeneratePdfFromHtmlAsync(htmlContent);
        }

        private string ProcessHtmlTemplate(string htmlTemplate, Dictionary<string, object> formData)
        {
            var processedHtml = htmlTemplate;

            foreach (var field in formData)
            {
                var placeholder = $"{{{{{field.Key}}}}}";
                var value = field.Value?.ToString() ?? "";
                processedHtml = processedHtml.Replace(placeholder, value);
            }

            // Remove any remaining placeholders
            processedHtml = Regex.Replace(processedHtml, @"\{\{[^}]+\}\}", "");

            return processedHtml;
        }
    }
}