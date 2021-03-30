using Autofac;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
