using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Requests;
using FlatRent.Repositories.Abstractions;
using FlatRent.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlatRent.Repositories
{
    public class AgreementRepository : AuthoredBaseRepository<Agreement>, IAgreementRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AgreementRepository(DataContext context, IMapper mapper, ILogger logger) : base(context, logger)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<FormError>, Guid)> AddAgreementAsync(Guid flatId, Guid userId,
            RentAgreementForm form)
        {
            var agreement = _mapper.Map<Agreement>(form);

            agreement.TenantId = userId;
            agreement.FlatId = flatId;
            agreement.StatusId = AgreementStatus.Statuses.Requested;
            agreement.Attachments.SetProperty(a => a.AuthorId, userId);

            var errors = await AddAsync(agreement, userId);
            return (errors, agreement.Id);
        }

        public async Task<IEnumerable<FormError>> CancelAgreementAsync(Guid id)
        {
            var agreement = await _context.Agreements.FindAsync(id).ConfigureAwait(false);
            if (agreement == null)
            {
                return FormError.CreateList(new FormError(Errors.AgreementNotFound));
            }

            return await DeleteAsync(agreement);
        }
    }
}