using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FlatRent.Controllers.Interfaces
{
    public interface IAuthoredEntityValidatingController
    {
        [NonAction]
        Task<IActionResult> IsAllowedToEditEntity(Guid id, string fieldId);

        [NonAction]
        Task<IActionResult> DoesEntityExistAsync(Guid id, string fieldId);
    }
}