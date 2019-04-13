using System.Collections.Generic;
using System.Linq;
using FlatRent.Extensions;
using FlatRent.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlatRent.Controllers
{
    public abstract class ErrorHandlingController : Controller
    {
        protected BadRequestObjectResult BadRequest(FormError error)
        {
            return base.BadRequest(FormatError(error));
        }

        protected BadRequestObjectResult BadRequest(IEnumerable<FormError> error)
        {
            return base.BadRequest(FormatError(error));
        }

        protected NotFoundObjectResult NotFound(FormError value)
        {
            return base.NotFound(FormatError(value));
        }

        protected NotFoundObjectResult NotFound(IEnumerable<FormError> value)
        {
            return base.NotFound(FormatError(value));
        }

        protected IActionResult HandleFormErrors(IEnumerable<FormError> errors, int successCode)
        {
            var formErrors = errors as FormError[] ?? errors?.ToArray() ?? new FormError[0];
            if (formErrors.Length > 0) return BadRequest(formErrors);
            return StatusCode(successCode);
        }

        private static object FormatError(object error)
        {
            switch (error)
            {
                case FormError formError:
                    return new[] { formError }.GetFormattedResponse();
                case IEnumerable<FormError> formErrors:
                    return formErrors.GetFormattedResponse();
            }

            return null;
        }
    }
}