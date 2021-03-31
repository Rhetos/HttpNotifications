using Autofac;
using Rhetos.Utilities;
using System.ComponentModel.Composition;

namespace Rhetos.HttpNotifications
{
    [Export(typeof(Module))]
    public class AutofacModuleConfiguration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => context.Resolve<IConfiguration>().GetOptions<HttpNotificationsOptions>()).SingleInstance();
            builder.RegisterType<HttpNotificationsDispatcher>().InstancePerLifetimeScope();
            builder.RegisterType<Subscriptions>().As<ISubscriptions>().InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
