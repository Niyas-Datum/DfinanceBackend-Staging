using System.IO;
using Microsoft.Extensions.Configuration;

namespace Dfinance.Shared.Configuration
{
    public static class ConfigurationManager
    {
        private static IConfigurationRoot configuration;
        
        static ConfigurationManager()
        {
           // Build();
        }
        public static string GetConnectionString(string name = "DataConnection")
        {
            string result = configuration.GetConnectionString(name);
            return result;
        }
        private static void Build()
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            configuration = builder.Build();
        }
    }
}