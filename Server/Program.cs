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

            if (Enumerable.SequenceEqual(args, new[] { "install" }))
            {
                WindowsServiceTool.RegisterToWindowsService(ServiceName, DisplayName, Description);
            }
            else if (Enumerable.SequenceEqual(args, new[] { "uninstall" }))
            {
                WindowsServiceTool.UnregisterToWindowsService(ServiceName);
            }
            else
            {
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
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
