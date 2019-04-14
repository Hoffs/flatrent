using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FlatRent.Constants;
using FlatRent.Controllers.Abstractions;
using FlatRent.Dtos;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Requests;
using FlatRent.Repositories.Interfaces;
using FlatRent.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace FlatRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ErrorHandlingController
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
            
            return Ok(new { OwnerAgreements = ownerAgreements, RenterAgreements = renterAgreements });
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
    }
}