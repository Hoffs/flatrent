﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Requests;
using FlatRent.Models.Requests.Flat;
using FlatRent.Repositories.Abstractions;
using FlatRent.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlatRent.Repositories
{
    public class FlatRepository : AuthoredBaseRepository<Flat>, IFlatRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FlatRepository(DataContext context, IMapper mapper, ILogger logger) : base(context, logger)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<FormError>, Guid)> AddFlatAsync(FlatForm form, Guid userId)
        {
            var flat = _mapper.Map<Flat>(form);
            flat.Address = _mapper.Map<Address>(form);
            flat.AuthorId = userId;
            flat.Address.AuthorId = userId;
            flat.Features = flat.Features.Select(f => f.Trim());
            var images = _mapper.Map<IEnumerable<FileMetadata>, IEnumerable<Image>>(form.Images).ToArray();
            images.SetProperty(i => i.AuthorId, userId);
            images.SetProperty(i => i.Flat, flat);

            flat.Images = new List<Image>(images);

            return (await AddAsync(flat, userId), flat.Id);
        }

        public async Task<IEnumerable<FormError>> DeleteAsync(Guid flatId)
        {
            var flat = await _context.Flats.FindAsync(flatId).ConfigureAwait(false);
            if (flat == null)
            {
                return new[] {new FormError("FlatId", Errors.FlatNotFound)};
            }

            return await DeleteAsync(flat);
        }

        public new async Task<IEnumerable<FormError>> UpdateAsync(Flat flat)
        {

            return await base.UpdateAsync(flat);
        }

        public async Task<IEnumerable<Flat>> GetListAsync(bool includeRented = false, int count = 20, int offset = 0)
        {
            var query = includeRented
                ? _context.Flats
                : _context.Flats.Where(x => !x.Agreements.Any(agreement => agreement.To > DateTime.UtcNow && !agreement.Deleted) && !x.Deleted);
            return query.OrderByDescending(x => x.CreatedDate).Skip(offset).Take(count);
        }

        public Task<int> GetCountAsync(bool includeRented = false)
        {
            var query = includeRented
                ? _context.Flats
                : _context.Flats.Where(x => !x.Agreements.Any(agreement => agreement.To > DateTime.UtcNow && !agreement.Deleted) && !x.Deleted);
            return query.CountAsync();
        }

        public async Task<IEnumerable<FormError>> AddAgreementTask(Guid flatId, Guid clientId, RentAgreementForm form)
        {
            var flat = await GetAsync(flatId).ConfigureAwait(false);
            if (flat?.IsAvailableForRent != true)
            {
                return new []{new FormError(Errors.FlatNotAvailableForRent)};
            }

            if (clientId == flat.AuthorId)
            {
                return new[] { new FormError(Errors.TenantCantBeOwner) };
            }

            var agreement = _mapper.Map<Agreement>(form);
            agreement.TenantId = clientId;
            agreement.FlatId = flatId;
            agreement.StatusId = AgreementStatus.Statuses.Requested;
            flat.Agreements.Add(agreement);

            return await UpdateAsync(flat);
        }
    }
}