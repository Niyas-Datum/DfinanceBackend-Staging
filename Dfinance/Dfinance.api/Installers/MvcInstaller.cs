using Dfinance.api.Installers;

namespace Dfinance.Shared.Installers;
public class MvcInstaller : IInstaller
{
    public void InstallService(IServiceCollection service, IConfiguration configuration)
    {
        service.AddCors();
        service.AddControllers();
        // // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        service.AddEndpointsApiExplorer();
        service.AddSwaggerGen();
    }
}