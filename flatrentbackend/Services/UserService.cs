using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Entities;
using FlatRent.Interfaces;
using FlatRent.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using JwtPayload = FlatRent.Models.JwtPayload;

namespace FlatRent.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, ILogger logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            _logger.Information("Authenticating {Email}", email);
            var user = await _userRepository.GetUserForCredentialsAsync(email, password).ConfigureAwait(false);
            if (user == null)
            {
                return null;
            }

            var jwtPayload = JwtPayload.CreateFromUser(user);
            return GenerateJwtToken(jwtPayload.GetAsClaims());
        }

        public async Task<string> RefreshAsync(ClaimsPrincipal claim)
        {
            _logger.Information("Refreshing {ClaimIdentity}", claim.Identity.Name);
            return GenerateJwtToken(claim.Claims);
        } 

        public async Task<ClaimsPrincipal> VerifyAsync(string token)
        {
            _logger.Information("Verifying {Token}", token);
            var key = Encoding.UTF8.GetBytes(_configuration["ServiceConfig:JwtSecret"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
            };

            var claims = tokenHandler.ValidateToken(token, parameters, out var validatedToken);
            return validatedToken.ValidTo >= DateTime.UtcNow && validatedToken.ValidFrom <= DateTime.UtcNow ? claims : null;
        }

        public Task<IEnumerable<FormError>> RegisterAsync(RegistrationForm data)
        {
            var user = _mapper.Map<User>(data);
            return _userRepository.AddClientAsync(user);
        }

        public Task<IEnumerable<FormError>> RegisterEmployeeAsync(RegistrationEmployeeForm data)
        {
            var user = _mapper.Map<User>(data);
            return _userRepository.AddEmployeeAsync(user);
        }

        private string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["ServiceConfig:JwtSecret"]);
            var filteredClaims = claims.Where(x => x.Type != "aud");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(filteredClaims),
                Audience = _configuration["ServiceConfig:Audience"],
                Issuer = _configuration["ServiceConfig:Issuer"],
                IssuedAt = DateTime.UtcNow.AddHours(-8),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}