using Microsoft.EntityFrameworkCore;
using newsetup.repos.ApiService;
using newsetup.repos.ApiService.Repository.Concrete;
using newsetup.repos.ApiService.Repository.Interfaces;
using newsetup.repos.ApiService.Repository;
using NodaTime;
using newsetup.repos.ApiService.Domain.HostedServices.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Register the DbContext
builder.Services.AddDbContext<NewRepoContext>((services, options) =>
{
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors()
           .UseLoggerFactory(loggerFactory);
});

builder.Services.AddDbContext<SharedContext>((services, options) =>
{
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    options.UseSqlServer(builder.Configuration.GetConnectionString("SharedConnection"))
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors()
           .UseLoggerFactory(loggerFactory);
});

// Register the IClock service
builder.Services.AddSingleton<IClock>(SystemClock.Instance);

// Register the InboxRepository
builder.Services.AddScoped<IInboxRepository, InboxRepository>();

// Register other services and configurations
builder.Services.Configure<ScopedBatchServiceOptions>(builder.Configuration.GetSection("ScopedBatchServiceOptions"));
builder.Services.Configure<UserUpdatedServiceOptions>(builder.Configuration.GetSection("UserUpdatedServiceOptions"));
builder.Services.AddAutoMapper(typeof(Program)); // Assuming you have AutoMapper profiles



var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();