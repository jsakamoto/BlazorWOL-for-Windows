﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorWOL.Server
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.Configuration);
            services.AddRazorPages();
            services.AddControllersWithViews();

            services.AddSingleton(serviceProvider =>
            {
                var baseDir = Path.Combine($"{Path.DirectorySeparatorChar}", "etc", "blazorWOL");
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BlazorWOL");

                baseDir = this.Configuration.GetValue("application-data-location", baseDir);
                baseDir = Path.GetFullPath(Path.Combine(baseDir, "."));

                if (!Directory.Exists(baseDir)) Directory.CreateDirectory(baseDir);
                var storagePath = Path.Combine(baseDir, "devices.json");

                return new DeviceStorage(storagePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
