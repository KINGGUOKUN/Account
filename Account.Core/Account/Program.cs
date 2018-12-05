using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Account
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("hostsettings.json", optional: true)
               .AddJsonFile("autofac.json", optional: true)
               .AddCommandLine(args)
               .Build();

            return WebHost.CreateDefaultBuilder(args)
                 .UseConfiguration(config)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddFilter("System", LogLevel.Warning);
                    logging.AddFilter("Microsoft", LogLevel.Warning);
                    logging.AddDebug();
                    logging.AddConsole();
                    logging.AddLog4Net();
                })
                .UseStartup<Startup>();
        }
    }
}
