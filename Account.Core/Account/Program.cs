using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

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
            .UseStartup<Startup>();
    }
}
