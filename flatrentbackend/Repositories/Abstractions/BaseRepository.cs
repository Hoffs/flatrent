using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlatRent.Repositories.Abstractions
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DataContext Context;
        protected readonly IMapper Mapper;
        protected readonly ILogger Logger;

        protected BaseRepository(DataContext context, IMapper mapper, ILogger logger)
        {
            Context = context;
            Mapper = mapper;
            Logger = logger;
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return Context.Set<TEntity>().AsNoTracking().AnyAsync(e => e.Deleted == false && e.Id == id);
        }

        public Task<TEntity> GetAsync(Guid id)
        {
            return Context.Set<TEntity>().FirstOrDefaultAsync(e => e.Deleted == false && e.Id == id);
        }

        protected async Task<IEnumerable<FormError>> AddAsync(TEntity entity)
        {
            try
            {
                await Context.AddAsync(entity).ConfigureAwait(false);
                await Context.SaveChangesAsync().ConfigureAwait(false);
                return null;
            }
            catch (DbUpdateException e)
            {
                Logger.Error(e, "Exception thrown while adding entity of type {EntityType} with {EntityId}", typeof(TEntity), entity.Id);
                return new[] { new FormError(Errors.Exception) };
            }
        }

        protected async Task<IEnumerable<FormError>> UpdateAsync(TEntity entity)
        {
            try
            {
                Context.Update(entity);
                await Context.SaveChangesAsync().ConfigureAwait(false);
                return null;
            }
            catch (DbUpdateException e)
            {
                Logger.Error(e, "Exception thrown while updating entity of type {EntityType} with {EntityId}", typeof(TEntity), entity.Id);
                return new[] { new FormError(Errors.Exception) };
            }
        }

        protected async Task<IEnumerable<FormError>> DeleteAsync(TEntity entity)
        {
            try
            {
                Context.Set<TEntity>().Delete(entity);
                await Context.SaveChangesAsync().ConfigureAwait(false);
                return null;
            }
            catch (DbUpdateException e)
            {
                Logger.Error(e, "Exception thrown while removing entity of type {EntityType} with {EntityId}", typeof(TEntity), entity.Id);
                return new[] { new FormError(Errors.Exception) };
            }
        }

        protected async Task<IEnumerable<FormError>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await Context.Set<TEntity>().FindAsync(id);
                return await DeleteAsync(entity);
            }
            catch (DbUpdateException e)
            {
                Logger.Error(e, "Exception thrown while removing entity of type {EntityType} with {EntityId}", typeof(TEntity), id);
                return new[] { new FormError(Errors.Exception) };
            }
        }
    }
}