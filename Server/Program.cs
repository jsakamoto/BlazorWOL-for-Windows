using System;
using System.Linq;
using System.Threading.Tasks;
using BlazorWOL.Server.Internals;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BlazorWOL.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var isWinSvcMode = args.Contains("--service");
            var hostBuilder = CreateHostBuilder(args);
            if (isWinSvcMode)
            {
                await hostBuilder.RunAsServiceAsync();
            }
            else
            {
                await hostBuilder.RunConsoleAsync();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
