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

        [Authorize(Roles = "Administrator")]
        [HttpPost("register/employee")]
        public async Task<IActionResult> RegisterEmployee(RegistrationEmployeeForm form)
        {
            try
            {
                var errors = await _userService.RegisterEmployeeAsync(form).ConfigureAwait(false);
                if (errors != null) return new BadRequestObjectResult(errors.GetFormattedResponse());
                return StatusCode(201);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Exception thrown while registering with {RegistrationEmployeeForm}", form);
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
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> GetAgreements(int count = 20, int offset = 0)
        {
            var userId = HttpContext.User.GetUserId();
            var user = await _repository.GetUser(userId).ConfigureAwait(false);
            if (user == null)
            {
                return BadRequest(new []{new FormError(Errors.Exception)}.GetFormattedResponse());
            }

            if (user.ClientInformationId == Guid.Empty)
            {
                return BadRequest(new []{new FormError(Errors.UserIsNotClient)}.GetFormattedResponse());
            }
            
            var agreements = user.ClientInformation?.Agreements.Where(x =>
                !x.Deleted && x.To >= DateTime.UtcNow.Date).AsQueryable().ProjectTo<RentAgreementListItem>(_mapper.ConfigurationProvider);
            return Ok(agreements);
        }

        [HttpGet("test/client")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> TestClient()
        {
            return StatusCode(200);
        }

        [HttpGet("test/admin")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> TestAdmin()
        {
            return StatusCode(200);
        }

        [HttpGet("test/employee")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> TestEmployee()
        {
            return StatusCode(200);
        }
    }
}