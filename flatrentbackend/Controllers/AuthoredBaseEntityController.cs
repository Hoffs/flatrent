using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatRent.Constants;
using FlatRent.Controllers.Interfaces;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Globals;
using FlatRent.Models;
using FlatRent.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlatRent.Controllers
{
    public abstract class AuthoredBaseEntityController<T> : ErrorHandlingController, IAuthoredEntityValidatingController where T : AuthoredBaseEntity
    {
        private readonly IBaseRepository<T> _repository;

        protected AuthoredBaseEntityController(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        [NonAction]
        public async Task<IActionResult> IsAllowedToEditEntity(Guid id, string fieldId)
        {
            var existsActionResult = await DoesEntityExistAsync(id, fieldId);
            if (existsActionResult != null) return existsActionResult;

            if (HttpContext.User?.GetUserId() == null) return Unauthorized();
            if (HttpContext.User.IsInRole(UserType.Administrator.Id.ToString())) return null;
            return await _repository.IsAuthorAsync(id, HttpContext.User.GetUserId())
                ? null
                : Unauthorized(new FormError(fieldId, Errors.NotAuthor));
        }


        [NonAction]
        public async Task<IActionResult> DoesEntityExistAsync(Guid id, string fieldId)
        {
            return await _repository.ExistsAsync(id)
                ? null
                : NotFound(new FormError(fieldId, Errors.FlatNotFound));
        }
    }
}