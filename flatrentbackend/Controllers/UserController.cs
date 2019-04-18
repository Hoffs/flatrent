using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FlatRent.Constants;
using FlatRent.Controllers.Abstractions;
using FlatRent.Controllers.Filters;
using FlatRent.Controllers.Interfaces;
using FlatRent.Dtos;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Requests;
using FlatRent.Repositories.Interfaces;
using FlatRent.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using Swashbuckle.AspNetCore.Annotations;

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

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login with credentials")]
        [Produces("application/json")]
        public async Task<IActionResult> LoginAsync(LoginForm form)
        {
            var token = await _userService.AuthenticateAsync(form.Email, form.Password).ConfigureAwait(false);
            if (token == null)
            {
                return BadRequest(new FormError(Errors.BadCredentials));
            }

            return Ok(new { Token = token });
        }

        [Authorize]
        [SwaggerOperation(Summary = "Refresh bearer token")]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync()
        {
            var newToken = await _userService.RefreshAsync(HttpContext.User).ConfigureAwait(false);
            if (newToken == null)
            {
                return BadRequest(new FormError(Errors.BadToken));
            }

            return Ok(new { Token = newToken });
        }

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register")]
        public async Task<IActionResult> Register(RegistrationForm form)
        {
            var errors = await _userService.RegisterAsync(form).ConfigureAwait(false);
            if (errors != null) return BadRequest(errors);
            return StatusCode(201);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSelfData()
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == Guid.Empty)
            {
                return BadRequest(new FormError(Errors.Exception));
            }

            var userData = await _repository.GetUser(userId).ConfigureAwait(false);
            return Ok(userData);
        }

        [HttpGet("{id}/avatar")]
        [AllowAnonymous]
        [EntityMustExist]
        public async Task<IActionResult> GetAvatar(Guid id)
        {
            var user = await _repository.GetUser(id);
            return File(user.Avatar.Bytes, user.Avatar.MimeType, user.Avatar.Name);
        }

        [HttpPut("{id}/avatar")]
        [EntityMustExist]
        public async Task<IActionResult> UpdateAvatar(Guid id, IFormFile image)
        {
            _logger.Debug("Uploading image with {Id}", id);
            using (var stream = image.OpenReadStream())
            {
                if (stream.Length > 1 * MemorySize.Megabyte)
                    return BadRequest(new FormError("File", Errors.FileTooBig));

                if (!image.IsImage())
                    return BadRequest(new FormError("File", Errors.InvalidImage));
                var user = await _repository.GetUser(id).ConfigureAwait(false);

                user.Avatar.Bytes = new byte[stream.Length];
                user.Avatar.MimeType = image.ContentType;
                await stream.ReadAsync(user.Avatar.Bytes);

                var errors = await _repository.UpdateAsync(user);
                if (errors != null) return BadRequest(errors);
                return Ok();
            }
        }


        [HttpGet("agreements")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> GetAgreements(int count = 20, int offset = 0)
        {
            var userId = HttpContext.User.GetUserId();
            var user = await _repository.GetUser(userId).ConfigureAwait(false);
            if (user == null)
            {
                return BadRequest(new FormError(Errors.Exception));
            }

            var ownerAgreements = user.OwnerAgreements.AsQueryable().ProjectTo<RentAgreementListItem>(_mapper.ConfigurationProvider);
            var renterAgreements = user.TenantAgreements.AsQueryable().ProjectTo<RentAgreementListItem>(_mapper.ConfigurationProvider);
            
            return Ok(new { owner = ownerAgreements, tenant = renterAgreements });
        }

        [HttpGet("test/roleclient")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> TestUser()
        {
            return StatusCode(200);
        }

        [HttpGet("test/roleadmin")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> TestAdmin()
        {
            return StatusCode(200);
        }

        [HttpGet("test/policyuser")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> TestPolicyUser()
        {
            return StatusCode(200);
        }

        [HttpGet("test/policyadmin")]
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> TestPolicyAdmin()
        {
            return StatusCode(200);
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