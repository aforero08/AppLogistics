using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace AppLogistics.Data;

public class Program
{
    public static void Main()
    {
    }

    public static IWebHost BuildWebHost(params string[] args)
    {
        return new WebHostBuilder()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .UseKestrel()
            .Build();
    }
}
