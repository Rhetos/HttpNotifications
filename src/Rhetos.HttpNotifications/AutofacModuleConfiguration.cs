using Autofac;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
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

            // TODO: Remove registrations of all IHttpNotificationSender after migrating to Rhetos 4.4, and use builder.GetRhetosConfiguration() to register only one selected implementation.
            builder.RegisterType<HttpNotificationSender>().InstancePerLifetimeScope();
            builder.RegisterType<HttpNotificationDatabaseLogger>().InstancePerLifetimeScope();
            builder.RegisterType<HttpNotificationSystemLogger>().InstancePerLifetimeScope();
            builder.Register<IHttpNotificationSender>(HttpNotificationSenderFactory).InstancePerLifetimeScope();

            base.Load(builder);
        }

        // TODO: Remove this factory after migrating to Rhetos 4.4, and use builder GetRhetosConfiguration to register only one selected IHttpNotificationSender implementation.
        private static IHttpNotificationSender HttpNotificationSenderFactory(IComponentContext context)
        {
            SendNotificationsMethod method = context.Resolve<HttpNotificationsOptions>().SendNotifications;
            return _senderImplementation[method](context);
        }

        private static readonly Dictionary<SendNotificationsMethod, Func<IComponentContext, IHttpNotificationSender>> _senderImplementation =
            new Dictionary<SendNotificationsMethod, Func<IComponentContext, IHttpNotificationSender>>
        {
            { SendNotificationsMethod.Http, context => context.Resolve<HttpNotificationSender>() },
            { SendNotificationsMethod.DatabaseLog, context => context.Resolve<HttpNotificationDatabaseLogger>() },
            { SendNotificationsMethod.SystemLog, context => context.Resolve<HttpNotificationSystemLogger>() },
        };
    }
}
