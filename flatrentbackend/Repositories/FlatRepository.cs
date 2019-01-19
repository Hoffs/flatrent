using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Interfaces;
using FlatRent.Models;
using FlatRent.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlatRent.Repositories
{
    public class FlatRepository : IFlatRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public FlatRepository(DataContext context, IMapper mapper, ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<FormError>> AddFlatAsync(FlatForm form)
        {
            var flat = _mapper.Map<Flat>(form);
            flat.Address = _mapper.Map<Address>(form);
            if (form.OwnerId != null)
            {
                var owner = await _context.Owners.FindAsync(form.OwnerId).ConfigureAwait(false);
                if (owner == null)
                {
                    return new[] {new FormError("OwnerId", Errors.OwnerNotFound)};
                }

                flat.Owner = owner;
            }
            else
            {
                flat.Owner = _mapper.Map<Owner>(form);
            }

            await _context.Flats.AddAsync(flat).ConfigureAwait(false);
            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return null;
            }
            catch (DbUpdateException e)
            {
                _logger.Error(e, "Exception thrown while creating flat with {CreatFlatForm}", form);
                return new[] {new FormError(Errors.Exception)};
            }
        }

        public async Task<IEnumerable<FormError>> DeleteFlatAsync(Guid flatId)
        {
            var flat = await _context.Flats.FindAsync(flatId).ConfigureAwait(false);
            if (flat == null)
            {
                return new[] {new FormError("FlatId", Errors.FlatNotFound)};
            }
            _context.Flats.Remove(flat);
            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return null;
            }
            catch (DbUpdateException e)
            {
                _logger.Error(e, "Exception thrown while removing flat with {Id}", flatId);
                return new[] {new FormError(Errors.Exception)};
            }
        }

        public Task<Flat> GetFlatAsync(Guid flatId)
        {
            return _context.Flats.FindAsync(flatId);
        }

        public async Task<IEnumerable<Flat>> GetFlatsAsync(bool includeRented = false, int count = 20, int offset = 0)
        {
            var query = includeRented
                ? _context.Flats
                : _context.Flats.Where(x => !x.Agreements.Any(agreement => agreement.To > DateTime.UtcNow && !agreement.Deleted) && !x.Deleted);
            return query.OrderByDescending(x => x.CreatedDate).Skip(offset).Take(count);
        }

        public Task<int> GetFlatCountAsync(bool includeRented = false)
        {
            var query = includeRented
                ? _context.Flats
                : _context.Flats.Where(x => !x.Agreements.Any(agreement => agreement.To > DateTime.UtcNow && !agreement.Deleted) && !x.Deleted);
            return query.CountAsync();
        }

        public async Task<IEnumerable<FormError>> AddAgreemenTask(Guid flatId, Guid userId, RentAgreementForm form)
        {
            try
            {
                var flat = await GetFlatAsync(flatId).ConfigureAwait(false);
                var isRented = flat.Agreements.Any(x => x.From.Date >= DateTime.Now.Date && x.To <= DateTime.Now.Date && !x.Deleted);
                if (isRented)
                {
                    return new []{new FormError(Errors.FlatAlreadyRented)};
                }

                var user = await _context.Users.FindAsync(userId).ConfigureAwait(false);
                if (user.ClientInformationId == null)
                {
                    return new []{new FormError(Errors.UserIsNotClient)};
                }

                var agreement = _mapper.Map<RentAgreement>(form);
                agreement.ClientInformationId = (Guid) user.ClientInformationId;
                agreement.FlatId = flatId;
                agreement.Verified = true;
                flat.Agreements.Add(agreement);

                await _context.SaveChangesAsync().ConfigureAwait(false);
                return new FormError[0];
            }
            catch (DbUpdateException e)
            {
                _logger.Error(e, "Exception thrown while creating flat agreement with {RentAgreementForm}", form);
                return new[] {new FormError(Errors.Exception)};
            }
        }
    }
}