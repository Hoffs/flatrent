using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Requests;
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

        public UserRepository(DataContext context, IMapper mapper, ILogger logger) : base(context, mapper, logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<User> GetUser(Guid id)
        {
            return _context.Users.FindAsync(id);
//            return _context.Users.FindAsync(id);
        }

        public Task<User> GetUserLoaded(Guid id)
        {
            return _context.Users
//                .Include(u => u.Flats).ThenInclude(f => f.Images)
//                .Include(u => u.OwnerAgreements)
//                .Include(u => u.TenantAgreements)
                .FirstOrDefaultAsync(u => u.Id == id);
//            return _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<Flat>> GetUserFlats(Guid id, bool includeNonPublished, int offset = 0)
        {
            var user = await _context.Users.FindAsync(id);
//            var flatsQuery = _context.Flats.Where(f => f.AuthorId == id && !f.Deleted).Include(f => f.Images);
//            profile.Flats = Mapper.ProjectTo<ShortFlatDetails>(flatsQuery);
            IQueryable<Flat> flats = _context.Flats
                .Where(f => f.AuthorId == id && !f.Deleted)
                .OrderByDescending(f => f.CreatedDate);

            return await flats.Paginate(offset, 16).ToListAsync();
//            var mappedFlats = Mapper.Map<IEnumerable<ShortFlatDetails>>(flats);
//            return _context.Users
//                .Include(u => u.Flats).ThenInclude(f => f.Images)
////                .Include(u => u.OwnerAgreements)
////                .Include(u => u.TenantAgreements)
//                .FirstOrDefaultAsync(u => u.Id == id);
//            return _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<Agreement>> GetUserAgreementsOwner(Guid id, int offset = 0)
        {
            return await _context.Agreements
                .Where(f => f.Flat.AuthorId == id && !f.Deleted)
                .OrderByDescending(f => f.CreatedDate)
                .Paginate(offset, 16)
                .ToListAsync();
        }

        public async Task<IEnumerable<Agreement>> GetUserAgreementsTenant(Guid id, int offset = 0)
        {
            return await _context.Agreements
                .Where(f => f.AuthorId == id && !f.Deleted)
                .OrderByDescending(f => f.CreatedDate)
                .Paginate(offset, 16)
                .ToListAsync();
        }

        public new async Task<IEnumerable<FormError>> UpdateAsync(User user)
        {
            return await base.UpdateAsync(user);
        }

        public async Task<IEnumerable<FormError>> UpdateAsync(Guid userId, UserUpdateForm form)
        {
            var user = await GetAsync(userId);
            user.About = form.About;
            user.BankAccount = form.BankAccount;
            user.PhoneNumber = form.PhoneNumber;
            return await UpdateAsync(user);
        }

        public async Task<IEnumerable<FormError>> AddUserAsync(User user)
        {
            var error = await CheckEmailAsync(user.Email).ConfigureAwait(false);
            if (error != null) return new[] {error};

            user.TypeId = UserType.User.Id;
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
            return existing != null ? new FormError("email", Errors.EmailAlreadyExists) : null;
        }

        private static string HashPassword(string password, string fname, string lname)
        {
            var salt = Encoding.UTF8.GetBytes(string.Concat(fname, lname));
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 1000, 32));
        }
    }
}