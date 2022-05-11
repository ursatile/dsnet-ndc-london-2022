# Autobarn
This is the sample application for Dylan Beattie's workshop on distributed systems with .NET. It's an Asp.NET Core web application based on very simple data model for advertising used cars for sale.

## Getting Started

Run the `Autobarn.Website` project. *(The sample projects are configured for .NET Core 3.1, but will run on .NET 5/6 or Framework 4.6 if you modify the .csproj files to change the project targets - see below)*

```
cd dotnet
cd Autobarn.Website
dotnet run
```

Browse to [http://localhost:5000](http://localhost:5000) and you should see the Autobarn homepage:

![image-20210519192001483](images/autobarn-homepage-screenshot.png)

Click the "All Cars" link (or go to http://localhost:5000/vehicles) and you should see a long list of car registration numbers.

Running RabbitMQ using Docker

As part of the workshop, we'll add some message queueing and publish/subscribe behaviour using EasyNetQ, which uses RabbitMQ as a message broker.

The code is set up to talk to a local RabbitMQ image using default credentials. To run RabbitMQ using Docker:

```bash
docker run -d --hostname rabbitmq --name rabbitmq -p 8080:15672 -p 5672:5672 -e RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=pass rabbitmq:3-management
```

*(Note that RabbitMQ uses the hostname internally to store data, so it's important to specify the same hostname each time you start the container if you want your queues to persist between container instances.)*

### Changing the .NET Target Version

You'll need to edit `Autobarn.Website\Autobarn.Website.csproj` and `Autobarn.Data\Autobarn.Data.csproj`

Find the line:

`<TargetFramework>netcoreapp3.1</TargetFramework>`

To run on .NET Framework 4.6, change this to: `<TargetFramework>net46</TargetFramework>`

To run on .NET 5, change this to: `<TargetFramework>net5.0</TargetFramework>`

To run on .NET 6, change this to: `<TargetFramework>net6.0</TargetFramework>`

(You can see a full list of .NET versions and target frameworks at [https://docs.microsoft.com/en-us/dotnet/standard/frameworks](https://docs.microsoft.com/en-us/dotnet/standard/frameworks))

### Using MS SQL Server

By default, Autobarn uses an in-memory database. You can also run Autobarn using an MS SQL Server database that's available as a Docker container image. Run the Docker image using:

```
docker run -p 1433:1433 -d ursatile/ursatile-workshops:autobarn-mssql2019-latest
```

Then change `DatabaseMode` to `sql` in `Autobarn.Website\appsettings.json`

