using System;
using System.Threading.Tasks;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlatRent.Validators
{
    public class BaseEntityValidator<T> where T : AuthoredBaseEntity
    {
        private readonly IBaseRepository<T> _repository;

        public BaseEntityValidator(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

//        public async Task<bool> IsAllowedToEditEntity(Guid id, Guid userId, string fieldId)
//        {
//            var existsActionResult = await DoesEntityExistAsync(id, fieldId);
//            if (existsActionResult != null) return existsActionResult;
//
//            return await _repository.IsAuthorAsync(id, userId)
//                ? null
//                : new Obje(new FormError(fieldId, Errors.NotAuthor));
//        }
//
//        public async Task<IActionResult> DoesEntityExistAsync(Guid id, string fieldId)
//        {
//            return await _repository.ExistsAsync(id)
//                ? null
//                : NotFound(new FormError(fieldId, Errors.FlatNotFound));
//        }
    }
}