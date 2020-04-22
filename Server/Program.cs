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
        private const string ServiceName = "BlazorWOL";

        private const string DisplayName = "Blazor Wakeup On Lan";

        private const string Description = "Provide a Web UI to sending Wakeup On Lan magic packet.";

        public static async Task Main(string[] args)
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (args.Contains("install"))
            {
                WindowsServiceTool.RegisterToWindowsService(ServiceName, DisplayName, Description, args);
            }
            else if (args.SequenceEqual(new[] { "uninstall" }))
            {
                WindowsServiceTool.UnregisterToWindowsService(ServiceName);
            }
            else
            {
                var isWinSvcMode = Enumerable.Range(0, args.Length).Any(n => args.Skip(n).Take(2).SequenceEqual(new[] { "--service", "true" }));
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
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
