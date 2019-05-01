using System.Threading.Tasks;
using FlatRent.Constants;
using FlatRent.Extensions;
using FlatRent.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;

namespace FlatRent.Utilities
{
    public class CustomExceptionHandler
    {
        public static void Configure(IApplicationBuilder appBuilder)
        {
            appBuilder.Run(Handle);
        }

        public static async Task Handle(HttpContext context)
        {
            var feature = context.Features.Get<IExceptionHandlerPathFeature>();
            var logger = context.RequestServices.GetService<ILogger>();
            logger.Error(feature.Error, $"Exception occured while request at path: {context.Request.Path.ToString()} from user: {context.User.GetUserId()}");

            var result = JsonConvert.SerializeObject(new[] { new FormError(Errors.Exception) }.GetFormattedResponse());
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(result);
        }
        
    }
}