# Rhetos.Events

Rhetos.Events provides an infrastructure for decoupling a feature that emits an event and a feature that processes it.

For example, any action or a save operation might emit an event, while a generic event handler might process event subscriptions to send update notifications to some other web service.

## Features

* DSL concepts for declaring and emitting events:
  * **Event** `<EventName> <EventDataType>` -
    Declares a new custom event type (for example, 'OrderApproved').
    EventDataType parameter is a C# type of the event data (for example, 'ICollection\<Guid\>').
    The event can then be emitted by calling IEventProcessing.EmitEvent method.
  * **Event** `<EventName>` -
    Adds a new custom event type. The event data type is `object` by default.
  * **EmitsCrudEvents** `<Entity>` -
    After saving changes emits standard CRUD events: "ModuleName_EntityName_Deleted", "ModuleName_EntityName_Updated" and "ModuleName_EntityName_Inserted".
    The event data contains IDs of the records, type "ICollection\<Guid\>".
* **Custom event emitters** can be placed anywhere in the application.
  For example see IEventProcessing.EmitEvent method calls in [Books.rhe](https://github.com/Rhetos/HttpNotifications/blob/main/test/TestApp/DslScripts/Books.rhe).
  * Emitting an event allows various event handlers to process them (for example, HTTP notifications to an external system).
* **Event handlers** are implemented as plugins for Rhetos.Events.
  For example, Rhetos.HttpNotifications sends notifications to services that are subscribed to certain event types.
  * The event handlers are executed synchronously, but they may generate asynchronous tasks or background jobs, depending on the implementation.

## Remarks

* There is no need to use the event processing for a specific event that is intended for a single specific event handler.
  The emitted events are expected to be handled by generic event handlers (for example, a handler that manages run-time event subscriptions),
  or to allow implementation of an event handler that has no information on which component emits the event.
* Naming convention for new events: Event should be named in passive form,
  since it represents something that happened.
  For example "Bookstore_Book_Inserted" is one of CRUD events created by using EmitsCrudEvents concept on entity Bookstore.Book.
* Avoid using IEnumerable\<T\> and LINQ queries as an event data, in order to simplify serialization/deserialization.
Use ICollection\<T\>, List\<T\> or an array instead.

# Rhetos.HttpNotifications

## Installation and configuration

Rhetos.HttpNotifications depends on Rhetos.Jobs.Abstractions to send notifications asynchronously.
For it to work, add an implementations of Rhetos background job processing to your applications,
for example **Rhetos.Jobs.Hangfire** NuGet package
(see the installation [instruction](https://github.com/Rhetos/Jobs/blob/master/Readme.md)).

## Features

* Sends HTTP POST notifications to subscriber URL, as specified in entity's table *RhetosHttpNotifications.Subscription*.
  Edit the data in the table to configure subscriptions for different event types.
  * In case there are multiple identical notifications generated within a single unit-of-work scope (single web request), only the last one of them will be sent.
  * Alternatively, the notifications may be sent directly without using the event processing mechanism.
  * The notifications contain event data in POST body (for example, entity name and ID for an *update* event).
* Guarantees:
  * The notification will not be sent until the database transaction that created the event is committed.
    This assures that there will be no false notifications for an operation that was not completed successfully (transaction rolled back).
  * The notification is stored before processing, and retried if the notification POST request fails.
    This assures that the notification will eventually be sent ("at-least once" model) or permanently
    visible in the jobs list if the subscriber is unavailable (until explicitly removed).
* For test environments it provides alternatives to sending HTTP requests,
  that log the notifications instead to database log or application's log (console, e.g.).
* See [HttpNotifications options](https://github.com/Rhetos/HttpNotifications/blob/main/src/Rhetos.HttpNotifications/HttpNotificationsOptions.cs) for configuration settings.

## Limitations

* In-order delivery of the notifications is not guaranteed.
* In rare cases, the same HTTP notification can be sent more then once.
  * The NotificationId property in the request body will be the same in case of repeated notification.
  * This issue is possible due to limitations of underlying task management subsystem. For example, in case of a database connection issue, the notification task could fail to update it's status to 'successful' after sending the HTTP notification.

## Additional considerations

Test environments

* It is important to avoid sending notifications to production subscribers from a test environment

Database update

* HTTP notifications could be send during deployment process (dbupdate),
  for example within recompute-on-deploy for data marked with KeepSynchronized.
  This is intended behavior: If the deployment process resulted with updating data that has active subscriptions,
  the subscribers should be notified.
* Depending on system configuration, and implementation specifics of Rhetos.Jobs, the notifications might be queued during deployment,
  and send later when the application starts, or executed immediately during deployment .

Debugging

* If using Rhetos.Jobs.Hangfire for background processing, any failing jobs will be shown in [HangFire](https://www.hangfire.io/) tables in database, with exception details in HangFire.State table.
* Avoid using IEnumerable\<T\> and LINQ queries as an event data, in order to simplify serialization/deserialization.
  They might result with JsonSerializationException exception from Hangfire.
  Use ICollection\<T\>, List\<T\> or an array instead.

## Contributing to Rhetos.HttpNotifications

### Initial development setup

1. Create an empty database on SQL Server.
   For example: "RhetosHttpNotifications" database on "localhost" SQL Server instance.

2. Create file `test\TestApp\rhetos-app.local.settings.json` and enter the created database connection string.
   For example:

	```json
    {
      "ConnectionStrings": {
        "ServerConnectionString": {
          "ConnectionString": "Data Source=localhost;Initial Catalog=RhetosHttpNotifications;Integrated Security=SSPI;"
        }
      }
    }
	```

### Build

Prerequisites: Initial development setup (see above).

1. Run `Build.bat`.

### Test

Prerequisites: Successful build.

1. Run `Test.bat`.
