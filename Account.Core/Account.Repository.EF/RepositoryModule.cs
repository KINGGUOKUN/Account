using Account.Repository.Contract;
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Repository.EF
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<ManifestRepository>().As<IManifestRepository>();
            //builder.RegisterType<DailyRepository>().As<IDailyRepository>();
            //builder.RegisterType<MonthlyRepository>().As<IMontylyRepository>();
            //builder.RegisterType<YearlyRepository>().As<IYearlyRepository>();

            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
