using System;
using System.Threading.Tasks;
using FlatRent.Constants;
using FlatRent.Controllers.Abstractions;
using FlatRent.Controllers.Filters;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlatRent.Controllers
{
    [Authorize]
    public class AttachmentController : AuthoredBaseEntityController<Attachment>
    {
        private readonly IAttachmentRepository _repository;
        private readonly ILogger _logger;

        public AttachmentController(IAttachmentRepository repository, ILogger logger) : base(repository)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [EntityMustExist]
        public async Task<IActionResult> GetAttachmentAsync([FromRoute] Guid id)
        {
            _logger.Debug("Requesting attachment with {Id}", id);

            var attachment = await _repository.GetAsync(id).ConfigureAwait(false);
            if (!attachment.IsUploaded) return NotFound(new FormError("Id", Errors.NotFound));

            return File(attachment.Bytes, attachment.MimeType, attachment.Name);
        }

        [HttpPut("{id}")]
        [MustBeEntityAuthor]
        public async Task<IActionResult> UploadAttachmentAsync([FromRoute] Guid id, IFormFile file)
        {
            _logger.Debug("Uploading file with {Id}", id);
            using (var fileStream = file.OpenReadStream())
            {
                if (fileStream.Length > 5 * MemorySize.Megabyte)
                    return BadRequest(new FormError("File", Errors.FileTooBig));

                var attachment = await _repository.GetAsync(id).ConfigureAwait(false);
                if (attachment.Bytes?.Length > 0)
                    return BadRequest(new FormError("Id", Errors.ImageAlreadyUploaded));

                attachment.Bytes = new byte[fileStream.Length];
                attachment.MimeType = file.ContentType;
                await fileStream.ReadAsync(attachment.Bytes);

                var errors = await _repository.UpdateAsync(attachment);
                if (errors != null) return BadRequest(errors);
                return Ok();
            }
        }
    }
}