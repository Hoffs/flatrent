using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Interfaces;
using FlatRent.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlatRent.Repositories
{
    public class AgreementRepository : IAgreementRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AgreementRepository(DataContext context, IMapper mapper, ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<FormError>> CancelAgreement(Guid id)
        {
            try
            {
                var agreement = await _context.RentAgreements.FindAsync(id).ConfigureAwait(false);
                if (agreement == null)
                {
                    return new []{ new FormError(Errors.AgreementNotFound) };
                }

                agreement.Deleted = true;
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return new FormError[0];
            }
            catch (DbUpdateException e)
            {
                _logger.Error(e, "Exception thrown while deleting agreement with {AgreementId}", id);
                return new[] {new FormError(Errors.Exception)};
            }
        }

        public Task<RentAgreement> GetAgreement(Guid id)
        {
            return _context.RentAgreements.FindAsync(id);
        }
    }
}