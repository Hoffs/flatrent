using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Repositories.Abstractions;
using FlatRent.Repositories.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlatRent.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;

        public UserRepository(DataContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<User> GetUser(Guid id)
        {
            return _context.Users.FindAsync(id);
        }

        /// <exception cref="Exception">Rethrows exception.</exception>
        public async Task<IEnumerable<FormError>> AddClientAsync(User client)
        {
            try
            {
                return await AddUserAsync(client, "Client").ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Exception thrown while adding {Client}", client);
                throw;
            }
        }

        public new async Task<IEnumerable<FormError>> UpdateAsync(User user)
        {
            return await base.UpdateAsync(user);
        }

        private async Task<IEnumerable<FormError>> AddUserAsync(User user, string type)
        {
            var error = await CheckEmailAsync(user.Email).ConfigureAwait(false);
            if (error != null) return new[] {error};

            user.Type = await _context.UserTypes.FirstAsync(x => x.Name.Equals(type)).ConfigureAwait(false);
            user.Password = HashPassword(user.Password, user.FirstName, user.LastName);

            await _context.Users.AddAsync(user).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return null;
        }

        public async Task<User> GetUserForCredentialsAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x =>
                string.Equals(x.Email, email, StringComparison.InvariantCultureIgnoreCase)).ConfigureAwait(false);
            
            if (user == null) return null;
            var hashed = HashPassword(password, user.FirstName, user.LastName);
            
            return hashed.Equals(user.Password) ? user : null;
        }

        private async Task<FormError> CheckEmailAsync(string email)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(x =>
                string.Equals(x.Email, email, StringComparison.InvariantCultureIgnoreCase)).ConfigureAwait(false);
            return existing != null ? new FormError("Email", Errors.EmailAlreadyExists) : null;
        }

        private static string HashPassword(string password, string fname, string lname)
        {
            var salt = Encoding.UTF8.GetBytes(string.Concat(fname, lname));
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 1000, 32));
        }
    }
}