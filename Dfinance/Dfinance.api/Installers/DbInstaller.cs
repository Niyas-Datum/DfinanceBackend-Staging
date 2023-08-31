using Dfinance.AuthCore.Infrastructure;
using Dfinance.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.api.Installers;

public class DbInstaller : IInstaller
{
    public void InstallService(IServiceCollection service, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        service.AddDbContext<AuthCoreContext>(options =>
        options.UseSqlServer(connectionString));

        var connectionString2 = configuration.GetConnectionString("MaindbConnection");
        service.AddDbContext<DFCoreContext>(options =>
        options.UseSqlServer(connectionString2));

    }
}
