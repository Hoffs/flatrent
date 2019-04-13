using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FlatRent.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : AuthoredBaseEntity
    {
        protected readonly DataContext Context;
        protected readonly ILogger Logger;

        protected BaseRepository(DataContext context, ILogger logger)
        {
            Context = context;
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

        public async Task<bool> IsAuthorAsync(Guid id, Guid createdBy)
        {
            return (await Context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id).ConfigureAwait(false))?.AuthorId == createdBy;
        }

        protected async Task<IEnumerable<FormError>> AddAsync(TEntity entity, Guid authorId)
        {
            try
            {
                entity.AuthorId = authorId;
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
        
//        public Task<IEnumerable<T>> GetListAsync()
    }
}