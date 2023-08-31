

using Microsoft.OpenApi.Models;

namespace Dfinance.api.Installers;

public class SwaggerInstaller : IInstaller
{
    public void InstallService(IServiceCollection service, IConfiguration configuration)
    {
        service.AddSwaggerGen(x =>
    {
        //x.SwaggerDoc("v1", new Info { Title = "head", Version = "hello" });
        var security = new Dictionary<string, IEnumerable<string>>{
            {"Bearer", new string[0]}
        };
        x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Auithorization header uisng The bearer scheme",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                 {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
    });


    }
}


public class Info
{
    public string? Title { get; set; }
    public string? Version { get; set; }

}