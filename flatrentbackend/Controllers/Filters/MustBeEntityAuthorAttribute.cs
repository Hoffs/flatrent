using System;
using System.Threading.Tasks;
using FlatRent.Controllers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace FlatRent.Controllers.Filters
{
    public class MustBeEntityAuthorAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is IAuthoredEntityValidatingController authoredBaseEntityController && context.ActionArguments.ContainsKey("id"))
            {
                var id = (Guid) context.ActionArguments["id"];
                var actionResult = await authoredBaseEntityController.IsAllowedToEditEntity(id, "id");
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
                Log.Logger.Error($"{nameof(MustBeEntityAuthorAttribute)} used in controller {context.Controller?.GetType().FullName} which does not implement {nameof(IAuthoredEntityValidatingController)}");
                context.Result = new StatusCodeResult(500);
            }
        }
    }
}