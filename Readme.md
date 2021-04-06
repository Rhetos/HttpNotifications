# Rhetos.Events

Naming convention: event should be named in passive form, since it represents something that happened, for example "Bookstore_Book_Inserted".

Avoid returning IEnumerable<T> and LINQ queries as an event data, in order to simplify serialization/deserialization.
Use ICollection<T>, List<T> or an array instead.

# Rhetos.HttpNotifications

## Installation and configuration

`<add key="owin:AutomaticAppStartup" value="false"/>`

Add an implementations of Rhetos background job processing, for example *Rhetos.Jobs.Hangfire* NuGet package.

## Features

...

* In case there are multiple identical notifications generated within a single unit-of-work scope (single web request), only the last one of them will be sent.

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
* Avoid returning IEnumerable<T> and LINQ queries as an event data, in order to simplify serialization/deserialization.
  They might result with JsonSerializationException exception from Hangfire.
  Use ICollection<T>, List<T> or an array instead.

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
