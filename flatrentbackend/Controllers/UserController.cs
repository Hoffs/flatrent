using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Controllers.Abstractions;
using FlatRent.Controllers.Filters;
using FlatRent.Controllers.Interfaces;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Dtos;
using FlatRent.Models.Requests;
using FlatRent.Repositories.Interfaces;
using FlatRent.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlatRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ErrorHandlingController, IIdentifiableEntityController
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserController(IUserService userService, IUserRepository repository, IMapper mapper, ILogger logger)
        {
            _userService = userService;
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("login"), Produces("application/json")]
        public async Task<IActionResult> LoginAsync(LoginForm form)
        {
            var token = await _userService.AuthenticateAsync(form.Email, form.Password).ConfigureAwait(false);
            if (token == null)
            {
                return BadRequest(new FormError(Errors.BadCredentials));
            }

            return Ok(new { Token = token });
        }

        [Authorize, HttpPost("refresh")]
        public IActionResult RefreshAsync()
        {
            var newToken = _userService.RefreshAsync(HttpContext.User);
            if (newToken == null)
            {
                return BadRequest(new FormError(Errors.BadToken));
            }

            return Ok(new { Token = newToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationForm form)
        {
            var errors = await _userService.RegisterAsync(form).ConfigureAwait(false);
            if (errors != null) return BadRequest(errors);
            return StatusCode(201);
        }

        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> GetProfileData([FromRoute] Guid id)
        {
            var userId = id == Guid.Empty ? User.GetUserId() : id;
            var userData = await _repository.GetUser(userId).ConfigureAwait(false);
            var mappedProfile = _mapper.Map<User, UserProfile>(userData);
            if (User.GetUserId() != id)
            {
                mappedProfile.BankAccount = null;
                mappedProfile.PhoneNumber = null;
            }
            return Ok(mappedProfile);
        }

        [HttpGet("{id}/agreements/owner"), EntityMustExist, Authorize]
        public async Task<IActionResult> GetUserAgreementsOwner(Guid id, int offset = 0)
        {
            var userId = HttpContext.User.GetUserId();
            if (userId != id) return Forbid();

            var ownerAgreements = await _repository.GetUserAgreementsOwner(userId, offset);

            return Ok(_mapper.Map<IEnumerable<ShortAgreementDetails>>(ownerAgreements));
        }

        [HttpGet("{id}/agreements/tenant"), EntityMustExist, Authorize]
        public async Task<IActionResult> GetUserAgreementsTenant(Guid id, int offset = 0)
        {
            var userId = HttpContext.User.GetUserId();
            if (userId != id) return Forbid();

            var tenantAgreements = await _repository.GetUserAgreementsTenant(userId, offset);

            return Ok(_mapper.Map<IEnumerable<ShortAgreementDetails>>(tenantAgreements));
        }

        [HttpGet("{id}/flats"), EntityMustExist, Authorize(Policy = "User")]
        public async Task<IActionResult> GetUserFlats(Guid id, int offset = 0)
        {
            var userId = HttpContext.User.GetUserId();
            var flats = await _repository.GetUserFlats(id, userId == id, offset);
            
            return Ok(_mapper.Map<IEnumerable<ShortFlatDetails>>(flats));
        }

        [HttpGet("{id}/avatar"), AllowAnonymous, EntityMustExist]
        public async Task<IActionResult> GetAvatar(Guid id)
        {
            var user = await _repository.GetUser(id);
            return File(user.Avatar.Bytes, user.Avatar.MimeType, user.Avatar.Name);
        }

        [HttpPut("{id}/avatar"), EntityMustExist]
        public async Task<IActionResult> UpdateAvatar(Guid id, IFormFile image)
        {
            _logger.Debug("Uploading image with {Id}", id);
            if (User.GetUserId() != id) return Forbid();
            using (var stream = image.OpenReadStream())
            {
                if (stream.Length > 1 * MemorySize.Megabyte)
                    return BadRequest(new FormError("File", Errors.FileTooBig));

                if (!image.IsImage())
                    return BadRequest(new FormError("File", Errors.InvalidImage));
                var user = await _repository.GetUser(id).ConfigureAwait(false);

                if (user.AvatarId == Guid.Parse("00000000-0000-0000-0000-000000000001"))
                {
                    user.Avatar = new Avatar
                    {
                        Name = "avatar"
                    };
                }
                user.Avatar.Bytes = new byte[stream.Length];
                user.Avatar.MimeType = image.ContentType;
                await stream.ReadAsync(user.Avatar.Bytes);

                var errors = await _repository.UpdateAsync(user);
                return OkOrBadRequest(errors, Ok(new { Id = id }));
            }
        }

        [HttpPut("{id}"), EntityMustExist]
        public async Task<IActionResult> UpdateUserAsync(Guid id, UserUpdateForm form)
        {
            _logger.Debug("Updating user with {Id}", id);
            if (User.GetUserId() != id) return Forbid();
            var errors = await _repository.UpdateAsync(id, form);
            return OkOrBadRequest(errors, Ok(new { Id = id }));
        }

        [NonAction]
        public async Task<IActionResult> DoesEntityExistAsync(Guid id, string fieldId)
        {
            return await _repository.GetUser(id) != null
                ? null
                : NotFound(new FormError(fieldId, Errors.NotFound));
        }
    }
}