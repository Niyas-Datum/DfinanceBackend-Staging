using System.Text;
using Dfinance.AuthAppllication.Authorization;
using Dfinance.Shared.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Dfinance.api.Installers;

public class JwtInstaller : IInstaller
{
    public void InstallService(IServiceCollection service, IConfiguration configuration)
    {
        service.Configure<AppSettings>(configuration.GetSection("AppSettings"));

        var appSettings = new AppSettings();
        configuration.Bind(nameof(appSettings), appSettings);
        service.AddSingleton(appSettings);

        service.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.JwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
        });

        service.AddScoped<IJwtSecret, JwtSecret>();


    }
}