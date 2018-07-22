using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Logging;

namespace Account
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateBuildWebHost(args).Build().Run();
        }

        public static IWebHostBuilder CreateBuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddFilter("System", LogLevel.Warning);
                logging.AddFilter("Microsoft", LogLevel.Warning);
                logging.AddLog4Net();
            })
            .UseStartup<Startup>();
    }
}
