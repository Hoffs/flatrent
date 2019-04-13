using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlatRent.Controllers
{
    public abstract class AuthoredBaseEntityController<T> : ErrorHandlingController where T : AuthoredBaseEntity
    {
        private readonly IBaseRepository<T> _repository;

        protected AuthoredBaseEntityController(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        protected async Task<IActionResult> IsAllowedToEditEntity(Guid id, string fieldId)
        {
            var existsActionResult = await DoesEntityExistAsync(id, fieldId);
            if (existsActionResult != null) return existsActionResult;

            return await _repository.IsAuthorAsync(id, HttpContext.User.GetUserId())
                ? null
                : Unauthorized(new FormError(fieldId, Errors.NotAuthor));
        }

        protected async Task<IActionResult> DoesEntityExistAsync(Guid id, string fieldId)
        {
            return await _repository.ExistsAsync(id)
                ? null
                : NotFound(new FormError(fieldId, Errors.FlatNotFound));
        }
    }
}