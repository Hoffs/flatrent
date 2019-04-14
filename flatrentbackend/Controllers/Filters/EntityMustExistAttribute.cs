using System;
using System.Threading.Tasks;
using FlatRent.Controllers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.SqlServer.Server;
using Serilog;

namespace FlatRent.Controllers.Filters
{
    public class EntityMustExistAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is IAuthoredEntityValidatingController validatingController && context.ActionArguments.ContainsKey("id"))
            {
                var id = (Guid)context.ActionArguments["id"];
                var actionResult = await validatingController.DoesEntityExistAsync(id, "id");
                if (actionResult != null)
                {
                    context.Result = actionResult;
                }
                else
                {
                    await next();
                }
            }
            else
            {
                Log.Logger.Error($"{nameof(EntityMustExistAttribute)} used in controller {context.Controller?.GetType().FullName} which does not implement {nameof(IAuthoredEntityValidatingController)}");
                context.Result = new StatusCodeResult(500);
            }
        }
    }
}