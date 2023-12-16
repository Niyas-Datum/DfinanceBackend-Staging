namespace Dfinance.api.Authorization;

using Dfinance.AuthAppllication.Dto;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
   
    public void OnAuthorization(AuthorizationFilterContext context)
    {
       
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        // authorization
        var user = (AuthResponseDto?)context.HttpContext.Items["User"];
        if (context.HttpContext.Items["Permission"] != null)
        {
            var permission = (bool)context.HttpContext.Items["Permission"];
            if (!permission)
                context.Result = new JsonResult(new { message = "You dont have permission" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
        if (user == null )
        {
            // not logged in or role not authorized
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}