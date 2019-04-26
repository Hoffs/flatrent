using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatRent.Constants;
using FlatRent.Extensions;
using FlatRent.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FlatRent.Utilities
{
    public class CustomModelErrorResponse : IActionResult
    {
        public Task ExecuteResultAsync(ActionContext context)
        {
            var modelStateEntries = context.ModelState.Where(e => e.Value.Errors.Count > 0).ToArray();
            var errors = new List<FormError>();

            if (modelStateEntries.Any())
            {
                foreach (var (key, value) in modelStateEntries)
                {
                    foreach (var modelStateError in value.Errors)
                    {
                        var error = new FormError
                        {
                            Name = key,
                            Message = modelStateError.ErrorMessage,
                        };

                        errors.Add(error);
                    }
                }
            }

            var response = new ApiErrorResponse
            {
                Message = Errors.BadRequest,
                Errors = errors.GetFormattedResponse(),
            };

            var json = JsonConvert.SerializeObject(response);

            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.WriteAsync(json);
            return Task.CompletedTask;
        }
    }
}