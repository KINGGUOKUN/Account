using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Configuration;
using Account.Repository.EF;
using Microsoft.EntityFrameworkCore;
using Account.Infrustures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Account
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddDbContext<AccountContext>(options =>
            //         options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), db => db.UseRowNumberForPaging()));
            //services.AddDbContext<AccountContext>(options =>
            //         options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContextPool<AccountContext>( // replace "YourDbContext" with the class name of your DbContext
                options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), // replace with your Connection String
                    mysqlOptions =>
                    {
                        mysqlOptions.ServerVersion(new Version(5, 7, 17), ServerType.MySql); // replace with your Server Version and Type
                    }
            ));
            services.AddScoped<DbContext>(provider => provider.GetService<AccountContext>());

            services.AddCors();
            // Add framework services.
            services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
                .AddJsonOptions(options => options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss")
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var builder = new ContainerBuilder();
            builder.Populate(services);
            var module = new ConfigurationModule(Configuration);
            builder.RegisterModule(module);
            this.Container = builder.Build();

            return new AutofacServiceProvider(this.Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, AccountContext context, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //app.UseCors(builder => builder.WithOrigins("http://localhost:65062")
            //                      .AllowAnyHeader().AllowAnyMethod());
            app.UseCors(builder => builder.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            DbInitializer.Initialize(context);

            appLifetime.ApplicationStopped.Register(() => this.Container.Dispose());
        }
    }
}
