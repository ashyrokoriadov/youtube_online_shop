using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using OnlineShop.Library.Constants;
using System;

namespace OnlineShop.ApiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDocker = environment == EnvironmentNames.Docker;
            var logger = NLogBuilder.ConfigureNLog(isDocker ? "nlog.docker.config" : "nlog.config").GetCurrentClassLogger();
            logger.Debug($"Is docker environment: {isDocker}; environment name: {environment}.");

            CreateHostBuilder(args)
                .UseNLog()
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
