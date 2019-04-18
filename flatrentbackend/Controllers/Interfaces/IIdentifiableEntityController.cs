using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FlatRent.Controllers.Interfaces
{
    public interface IIdentifiableEntityController
    {
        [NonAction]
        Task<IActionResult> DoesEntityExistAsync(Guid id, string fieldId);
    }
}