using Account.Service.Contract;
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Service
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<ManifestService>().As<IManifestService>();
            //builder.RegisterType<DailyService>().As<IDailyService>();
            //builder.RegisterType<MonthlyService>().As<IMonthlyService>();
            //builder.RegisterType<YearlyService>().As<IYearlyService>();

            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerRequest();
        }
    }
}
