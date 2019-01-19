using System.Net;
using FlatRent.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace FlatRent.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
 
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    { 
                        logger.Error($"Something went wrong: {contextFeature.Error}");
 
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new 
                        {
                            context.Response.StatusCode,
                            Message = Errors.Exception
                        })).ConfigureAwait(false);
                    }
                });
            });
        }
    }
}