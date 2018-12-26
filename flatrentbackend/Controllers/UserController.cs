using System;
using System.Threading.Tasks;
using FlatRent.Extensions;
using FlatRent.Interfaces;
using FlatRent.Models;
using FlatRent.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        private readonly ILogger _logger;

        public UserController(IUserService userService, ILogger logger)
        {
            _userService = userService;
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
                    return BadRequest(new { Errors = new[] {new FormError("Invalid credentials.")}});
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
                    return BadRequest(new { Errors = new[] {new FormError("Invalid token.")}});
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