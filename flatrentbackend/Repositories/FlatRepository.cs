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

        public async Task<IEnumerable<FormError>> AddFlatAsync(FlatForm form, Guid ownerId)
        {
            var flat = _mapper.Map<Flat>(form);
            flat.Address = _mapper.Map<Address>(form);
            flat.OwnerId = ownerId;

            try
            {
                await _context.Flats.AddAsync(flat).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return null;
            }
            catch (DbUpdateException e)
            {
                _logger.Error(e, "Exception thrown while creating flat with {CreateFlatForm}", form);
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

        public async Task<IEnumerable<FormError>> AddAgreementTask(Guid flatId, Guid renterId, RentAgreementForm form)
        {
            try
            {
                var flat = await GetFlatAsync(flatId).ConfigureAwait(false);
                if (flat.IsRented || !flat.IsPublished)
                {
                    return new []{new FormError(Errors.FlatNotAvailableForRent)};
                }

                if (renterId == flat.OwnerId)
                {
                    return new[] { new FormError(Errors.RenterCantBeOwner) };
                }

                var agreement = _mapper.Map<Agreement>(form);
                agreement.RenterId = (Guid) renterId;
                agreement.FlatId = flatId;
                agreement.StatusId = AgreementStatus.Statuses.Requested;
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