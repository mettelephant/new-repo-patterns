using Aspire.Hosting.ApplicationModel;

var builder = DistributedApplication.CreateBuilder(args);

// Add MSSQL server with three databases: shared, 001, 002
var mssqlServer = builder.AddSqlServer("mssqlserver");
var sharedDatabase = mssqlServer.AddDatabase("shared");
var firstDatabase = mssqlServer.AddDatabase("001");
var secondDatabase = mssqlServer.AddDatabase("002");

// Add Postgres server with a database: NewRepo
var postgresServer = builder.AddPostgres("postgresserver");
var postgresDatabase = postgresServer.AddDatabase("NewRepo");

var kafka = builder.AddKafka("kafka");

var rabbitMq = builder.AddRabbitMQ("rabbitmq");

var apiService = builder.AddProject<Projects.newsetup_repos_ApiService>("apiservice")
    .WithReference(postgresDatabase, "DefaultConnection")
    .WaitFor(postgresDatabase)
    .WithReference(sharedDatabase, "SharedConnection")
    .WaitFor(sharedDatabase)
    .WithReference(firstDatabase)
    .WaitFor(firstDatabase)
    .WithReference(secondDatabase)
    .WaitFor(secondDatabase)
    .WithReference(kafka)
    .WaitFor(kafka)
    .WithReference(rabbitMq)
    .WaitFor(rabbitMq);

builder.AddProject<Projects.newsetup_repos_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();