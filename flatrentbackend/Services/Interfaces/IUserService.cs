using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FlatRent.Models;
using FlatRent.Models.Requests;

namespace FlatRent.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Tries to authenticate with given credentials and returns JWT Token
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns>JWT Token</returns>
        Task<string> AuthenticateAsync(string email, string password);
        
        /// <summary>
        /// Tries to refresh given JWT token
        /// </summary>
        /// <param name="token">existing JWT token</param>
        /// <returns>JWT Token</returns>
        Task<string> RefreshAsync(ClaimsPrincipal claim);

        /// <summary>
        /// Verifies validity of the token
        /// </summary>
        /// <param name="token">JWT Token</param>
        /// <returns>Is valid</returns>
        Task<ClaimsPrincipal> VerifyAsync(string token);

        Task<IEnumerable<FormError>> RegisterAsync(RegistrationForm data);
    }
}