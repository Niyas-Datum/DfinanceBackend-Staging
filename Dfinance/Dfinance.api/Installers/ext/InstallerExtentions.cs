namespace Dfinance.api.Installers.ext;

public static class InstallerExtesnions
{
    public static void InstallerServiceInAssembly(this IServiceCollection services, IConfiguration configuration)
    {
        var DependancyInstaller = typeof(Program).Assembly.ExportedTypes.Where(x => typeof(IInstaller).IsAssignableFrom(x)
                        && !x.IsInterface && !x.IsAbstract)
                        .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

        DependancyInstaller.ForEach(installer => installer.InstallService(services, configuration));

    }
}