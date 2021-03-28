# Rhetos.HttpNotifications

## Using Rhetos.HttpNotifications

### Installation and configuration

`<add key="owin:AutomaticAppStartup" value="false"/>`

Add an implementations of Rhetos background job processing, for example *Rhetos.Jobs.Hangfire* NuGet package.

###  Features

...

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
