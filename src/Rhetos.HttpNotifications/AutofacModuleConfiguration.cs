/*
    Copyright (C) 2014 Omega software d.o.o.

    This file is part of Rhetos.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

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
