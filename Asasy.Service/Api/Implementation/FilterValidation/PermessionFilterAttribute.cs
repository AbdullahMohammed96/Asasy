using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.Api.Implementation.FilterValidation
{
    public class PermessionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //if (!AllowAnonymous(context))
            //{
                var userId = context.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;

                if (userId != null)
                {
                    var dbContext = context.HttpContext.RequestServices.GetRequiredService<Persistence.ApplicationDbContext>();

                    var user = dbContext.Users.Where(u => u.Id == userId).FirstOrDefault();
                    if (user != null)
                    {
                        if (!user.IsActive)
                        {
                            context.Result = new StatusCodeResult(403); // Set a 403 Forbidden result
                            return;
                        }
                    }
                    else
                    {
                        context.Result = new StatusCodeResult(403); // Set a 403 Forbidden result
                        return;
                    }
                }



            //}

        }


        private bool AllowAnonymous(ActionExecutingContext context)
        {
            var allowAnonymousAttribute = context.ActionDescriptor.EndpointMetadata
                .OfType<AllowAnonymousAttribute>()
                .FirstOrDefault();

            return allowAnonymousAttribute != null;
        }
    }
}
