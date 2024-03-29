﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlatRent.Repositories.Abstractions
{
    public class AuthoredBaseRepository<TEntity> : BaseRepository<TEntity>, IAuthoredRepository<TEntity> where TEntity : AuthoredBaseEntity
    {
        public AuthoredBaseRepository(DataContext context, IMapper mapper, ILogger logger) : base(context, mapper, logger)
        {
        }

        public async Task<bool> IsAuthorAsync(Guid id, Guid createdBy)
        {
            return (await Context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id && e.AuthorId == createdBy).ConfigureAwait(false)) != null;
        }

        protected async Task<IEnumerable<FormError>> AddAsync(TEntity entity, Guid authorId)
        {
            entity.AuthorId = authorId;
            return await base.AddAsync(entity);
        }
    }
}