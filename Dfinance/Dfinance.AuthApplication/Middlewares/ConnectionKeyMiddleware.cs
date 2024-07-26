using Dfinance.Shared.Configuration.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.AuthApplication.Middlewares
{
    public class ConnectionKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConnectionServices _connectionServices;
        public ConnectionKeyMiddleware(RequestDelegate next, ILoggerFactory loggerFactory,
            IConnectionServices connectionServices
            )
        {
            _next = next;
            _connectionServices = connectionServices;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var key = context.Request.Headers["skey"];
            _connectionServices.setconkey(key);
            await this._next(context);
        }
    }
    public static class ConnectionKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseConnectionKey(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ConnectionKeyMiddleware>();
        }
    }
}
