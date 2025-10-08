using AppLogistics.Data.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace AppLogistics.Data
{
    public class Startup
    {
        private IConfiguration Config { get; }

        public Startup(IWebHostEnvironment env)
        {
            Config = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .SetBasePath(Directory.GetParent(env.ContentRootPath).FullName)
                .AddEnvironmentVariables("APPLOGISTICS_")
                .AddJsonFile("AppLogistics.Web/configuration.json")
                .AddJsonFile($"AppLogistics.Web/configuration.{env.EnvironmentName.ToLower()}.json", optional: true)
                .Build();
        }

        public void Configure()
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(options => options.UseSqlServer(Config["Data:Connection"]));
        }
    }
}
