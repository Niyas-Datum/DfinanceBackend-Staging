using Dfinance.AuthAppllication.Authorization;
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
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtSecret.ValidateJwtToken(token);
        if (userId != null)
        {
            // attach user to context on successful jwt validation
            context.Items["User"] = authservice.GetUserById(userId);
        }

        await _next(context);

    }

}
