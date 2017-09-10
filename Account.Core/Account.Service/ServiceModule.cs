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
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                //.Where(t => t.Name.EndsWith("Service"))
                .Where(t => typeof(IService).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
