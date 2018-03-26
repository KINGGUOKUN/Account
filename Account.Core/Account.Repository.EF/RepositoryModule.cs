using Account.Infrustructure.Contract;
using Account.Infrusture.EF;
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
            builder.RegisterType<EFUnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.IsAssignableTo<IRepository>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
