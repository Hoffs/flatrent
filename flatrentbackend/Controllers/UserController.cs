using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FlatRent.Constants;
using FlatRent.Dtos;
using FlatRent.Extensions;
using FlatRent.Interfaces;
using FlatRent.Models;
using FlatRent.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Serilog;

namespace FlatRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
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
        public async Task<IActionResult> LoginAsync(LoginForm form)
        {
            try
            {
                var token = await _userService.AuthenticateAsync(form.Email, form.Password).ConfigureAwait(false);
                if (token == null)
                {
                    return BadRequest(new[] {new FormError(Errors.BadCredentials)}.GetFormattedResponse());
                }
                return new OkObjectResult(new {Token = token});
            }
            catch (Exception e)
            {
                _logger.Error(e, "Exception thrown while logging in");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync()
        {
            try
            {
                var newToken = await _userService.RefreshAsync(HttpContext.User).ConfigureAwait(false);
                if (newToken == null)
                {
                    return BadRequest(new[] {new FormError(Errors.BadToken)}.GetFormattedResponse());
                }
                return new OkObjectResult(new {Token = newToken});
            }
            catch (Exception e)
            {
                _logger.Error(e, "Exception thrown while refreshing token");
                return StatusCode(500);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationForm form)
        {
            try
            {
                var errors = await _userService.RegisterAsync(form).ConfigureAwait(false);
                if (errors != null) return new BadRequestObjectResult(errors.GetFormattedResponse());
                return StatusCode(201);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Exception thrown while registering with {RegistrationForm}", form);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSelfData()
        {
            var userId = HttpContext.User.GetUserId();
            if (userId == Guid.Empty)
            {
                return BadRequest(new []{new FormError(Errors.Exception)}.GetFormattedResponse());
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
                return BadRequest(new []{new FormError(Errors.Exception)}.GetFormattedResponse());
            }

            var ownerAgreements = user.OwnerAgreements.AsQueryable().ProjectTo<RentAgreementListItem>(_mapper.ConfigurationProvider);
            var renterAgreements = user.RenterAgreements.AsQueryable().ProjectTo<RentAgreementListItem>(_mapper.ConfigurationProvider);
            
            // TODO: Differentiate OWNER/RENTER agreements

            return Ok(ownerAgreements.Concat(renterAgreements));
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