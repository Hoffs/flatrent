using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FlatRent.Controllers.Interfaces
{
    public interface IAuthoredEntityController
    {
        [NonAction]
        Task<IActionResult> IsAllowedToEditEntity(Guid id, string fieldId);
    }
}