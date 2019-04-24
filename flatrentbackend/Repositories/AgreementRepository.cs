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

        public async Task<IEnumerable<FormError>> CancelAgreement(Guid id)
        {
            var agreement = await _context.Agreements.FindAsync(id).ConfigureAwait(false);
            if (agreement == null)
            {
                return FormError.CreateList(new FormError(Errors.AgreementNotFound));
            }

            return await DeleteAsync(agreement);
        }

        public async Task<IEnumerable<FormError>> CreateAgreementTask(Guid flatId, Guid userId,
            RentAgreementForm form)
        {
            var agreement = _mapper.Map<Agreement>(form);

            var images = _mapper.Map<IEnumerable<FileMetadata>, IEnumerable<Attachment>>(form.Attachments).ToArray();
            images.SetProperty(i => i.AuthorId, userId);
            images.SetProperty(i => i.Agreement, agreement);

            agreement.TenantId = userId;
            agreement.FlatId = flatId;
            agreement.StatusId = AgreementStatus.Statuses.Requested;

            return await AddAsync(agreement, userId);
        }
    }
}