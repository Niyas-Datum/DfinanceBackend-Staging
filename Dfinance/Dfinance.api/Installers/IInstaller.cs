using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dfinance.api.Installers;

public interface IInstaller
{
    void InstallService(IServiceCollection service, IConfiguration configuration);
}
