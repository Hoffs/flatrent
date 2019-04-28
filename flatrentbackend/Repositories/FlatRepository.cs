using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Dtos;
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

        public FlatRepository(DataContext context, IMapper mapper, ILogger logger) : base(context, mapper, logger)
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
            flat.IsPublished = true;
            
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

        public IQueryable<Flat> GetListAsync(bool includeRented = false, int count = 20, int offset = 0)
        {
            IQueryable<Flat> query = _context.Flats.Where(x => !x.Deleted).OrderByDescending(x => x.CreatedDate);
            if (!includeRented)
            {
                query = query.Where(Flat.HasNoActiveAgreement);
            }
            query = query.Skip(offset).Take(count).Include(x => x.Images);
            query.Load();
            return query;
        }

        public Task<int> GetCountAsync(bool includeRented = false)
        {
            IQueryable<Flat> query = _context.Flats.Where(x => !x.Deleted).OrderByDescending(x => x.CreatedDate);
            if (!includeRented)
            {
                //                query = query.Where(x => x.ActiveAgreement == null);
                query = query.Where(Flat.HasNoActiveAgreement);
            }
            return query.AsTracking().CountAsync();
        }
    }
}