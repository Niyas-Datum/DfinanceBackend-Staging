using Dfinance.AuthAppllication.Authorization;
using Dfinance.AuthAppllication.Services;
using Dfinance.AuthAppllication.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace Dfinance.AuthAppllication.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IAuthService authservice, IJwtSecret jwtSecret)
    {
        try
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtSecret.ValidateJwtToken(token);
            var QueryData = context.Request.Query.ToList();
            var currentpageid = context.Request.Query["pageid"].ToString();
            var currentpermission = context.Request.Query["pagemethod"].ToString();

            if (token != null && currentpageid != "" && currentpermission != "")
            {
                context.Items["Permission"] = authservice.UserPermCheck(Convert.ToInt32(currentpageid), Convert.ToInt32(currentpermission));
            }
            else
            {
                context.Items["Permission"] = true;
            }
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = authservice.GetUserById(userId);
            }
        }catch (Exception ex)
        {

        }
        await _next(context);

    }

}
