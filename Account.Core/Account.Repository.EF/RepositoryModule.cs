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
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                //.Where(t => t.Name.EndsWith("Repository"))
                .Where(t => typeof(IRepository).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
