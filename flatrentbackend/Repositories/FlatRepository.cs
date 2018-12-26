using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Entities;
using FlatRent.Interfaces;
using FlatRent.Models;
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
                    return new[] {new FormError("OwnerId", "Owner not found")};
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
                _context.Flats.Remove(flat);
                return new[] {new FormError("Unexpected error occured when creating flat record.")};
            }
        }

        public async Task<IEnumerable<FormError>> DeleteFlatAsync(Guid flatId)
        {
            var flat = await _context.Flats.FindAsync(flatId).ConfigureAwait(false);
            if (flat == null)
            {
                return new[] {new FormError("FlatId", "Flat was not found.")};
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
                _context.Flats.Remove(flat);
                return new[] {new FormError("Unexpected error occured when deleting flat record.")};
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
                : _context.Flats.Where(x => !x.Agreements.Any(agreement => agreement.To > DateTime.UtcNow));
            return query.Skip(offset).Take(count);
        }

        public Task<int> GetFlatCountAsync(bool includeRented = false)
        {
            var query = includeRented
                ? _context.Flats
                : _context.Flats.Where(x => !x.Agreements.Any(agreement => agreement.To > DateTime.UtcNow));
            return query.CountAsync();
        }
    }
}