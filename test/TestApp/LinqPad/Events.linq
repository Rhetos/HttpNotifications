<Query Kind="Program">
  <Reference Relative="..\bin\Autofac.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Autofac.dll</Reference>
  <Reference Relative="..\bin\Autofac.Integration.Wcf.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Autofac.Integration.Wcf.dll</Reference>
  <Reference Relative="..\bin\EntityFramework.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\EntityFramework.dll</Reference>
  <Reference Relative="..\bin\EntityFramework.SqlServer.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\EntityFramework.SqlServer.dll</Reference>
  <Reference Relative="..\bin\Hangfire.AspNet.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Hangfire.AspNet.dll</Reference>
  <Reference Relative="..\bin\Hangfire.Core.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Hangfire.Core.dll</Reference>
  <Reference Relative="..\bin\NLog.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\NLog.dll</Reference>
  <Reference Relative="..\bin\Oracle.ManagedDataAccess.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Oracle.ManagedDataAccess.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Configuration.Autofac.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Configuration.Autofac.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Dom.DefaultConcepts.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Dom.DefaultConcepts.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Dom.DefaultConcepts.Interfaces.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Dom.DefaultConcepts.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Dom.Interfaces.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Dom.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Dsl.DefaultConcepts.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Dsl.DefaultConcepts.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Dsl.Interfaces.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Dsl.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Events.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Events.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Interfaces.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Jobs.Abstractions.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Jobs.Abstractions.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Jobs.Hangfire.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Jobs.Hangfire.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Logging.Interfaces.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Logging.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Persistence.Interfaces.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Persistence.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Processing.DefaultCommands.Interfaces.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Processing.DefaultCommands.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Processing.Interfaces.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Processing.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Security.Interfaces.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Security.Interfaces.dll</Reference>
  <Reference Relative="..\bin\Rhetos.TestCommon.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.TestCommon.dll</Reference>
  <Reference Relative="..\bin\Rhetos.Utilities.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\Rhetos.Utilities.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.AccountManagement.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.dll</Reference>
  <Reference Relative="..\bin\TestApp.dll">C:\My Projects\RhetosPackages\HttpNotifications\test\TestApp\bin\TestApp.dll</Reference>
  <Namespace>Autofac</Namespace>
  <Namespace>Oracle.ManagedDataAccess.Client</Namespace>
  <Namespace>Rhetos</Namespace>
  <Namespace>Rhetos.Configuration.Autofac</Namespace>
  <Namespace>Rhetos.Dom</Namespace>
  <Namespace>Rhetos.Dom.DefaultConcepts</Namespace>
  <Namespace>Rhetos.Dsl</Namespace>
  <Namespace>Rhetos.Dsl.DefaultConcepts</Namespace>
  <Namespace>Rhetos.Events</Namespace>
  <Namespace>Rhetos.Jobs.Hangfire</Namespace>
  <Namespace>Rhetos.Logging</Namespace>
  <Namespace>Rhetos.Persistence</Namespace>
  <Namespace>Rhetos.Security</Namespace>
  <Namespace>Rhetos.TestCommon</Namespace>
  <Namespace>Rhetos.Utilities</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Data.Entity</Namespace>
  <Namespace>System.DirectoryServices</Namespace>
  <Namespace>System.DirectoryServices.AccountManagement</Namespace>
  <Namespace>System.IO</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Reflection</Namespace>
  <Namespace>System.Runtime.Serialization.Json</Namespace>
  <Namespace>System.Text</Namespace>
  <Namespace>System.Xml</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
  <Namespace>Autofac.Integration.Wcf</Namespace>
</Query>

string applicationFolder = Path.GetDirectoryName(Util.CurrentQueryPath);

void Main()
{
	ConsoleLogger.MinLevel = EventType.Trace;
	RhetosJobsService.InitializeJobServer(GetRootContainer());

	using (var scope = ProcessContainer.CreateTransactionScopeContainer(applicationFolder))
	{
		var repository = scope.Resolve<Common.DomRepository>();
		var eventProcessing = scope.Resolve<IEventProcessing>();
		
		// Available events for subscriptions:
		
		eventProcessing.GetEventNames().Dump();
		
		// HTTP notifications Subscription:
		
		repository.RhetosHttpNotifications.Subscription.Load().Dump();
		
		// Emit event:
		
		var eventData = repository.Bookstore.Book.Load().Take(3).ToList();
		
		for (int i = 0; i < 5; i++)
		{
			eventProcessing.EmitEvent(
				Rhetos.Events.EventName.Bookstore_Book_Inserted, // Type-safe constants.
				eventData.Select(book => book.ID).ToList());

			eventProcessing.EmitEvent(
				Rhetos.Events.EventName.Bookstore_Book_InsertedImportantBook, // Type-safe constants.
				eventData);

			// Duplication notification are removed within a scope. This one has different data each time. 
			eventProcessing.EmitEvent(
				Rhetos.Events.EventName.Bookstore_Book_InsertedImportantBook, // Type-safe constants.
				i + 1);
		}
		
		// HTTP notifications are part of the atomic transaction of the main operation that created them.
		
		scope.CommitChanges();
    }
}

IContainer GetRootContainer()
{
	using (var scope = ProcessContainer.CreateTransactionScopeContainer(applicationFolder))
	{
		var processContainerField = typeof(ProcessContainer).GetField("_singleContainer", BindingFlags.NonPublic | BindingFlags.Static);
		var processContainer = (ProcessContainer)processContainerField.GetValue(null);

		var containerField = typeof(ProcessContainer).GetField("_rhetosIocContainer", BindingFlags.NonPublic | BindingFlags.Instance);
		var container = (Lazy<IContainer>)containerField.GetValue(processContainer);

		return container.Value;
	}
}