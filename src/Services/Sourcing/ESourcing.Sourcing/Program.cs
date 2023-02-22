using ESourcing.Sourcing.Data.Abstract;
using ESourcing.Sourcing.Data.Concrete;
using ESourcing.Sourcing.Repositories.Abstract;
using ESourcing.Sourcing.Repositories.Concrete;
using ESourcing.Sourcing.Settings.Abstract;
using ESourcing.Sourcing.Settings.Concrete;
using EventBusRabbitMQ.Abstract;
using EventBusRabbitMQ.Concrete;
using EventBusRabbitMQ.Producer;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region MyConfigs
builder.Services.Configure<SourcingDatabaseSettings>(builder.Configuration.GetSection(nameof(SourcingDatabaseSettings)));
builder.Services.AddSingleton<ISourcingDatabaseSettings>(sp=>sp.GetRequiredService<IOptions<SourcingDatabaseSettings>>().Value);
builder.Services.AddTransient<ISourcingContext, SourcingContext>();
builder.Services.AddTransient<IAuctionRepository, AuctionRepository>();
builder.Services.AddTransient<IBidRepository, BidRepository>();

//Event Bus DI
builder.Services.AddSingleton<IRabbitMQPersistentConnection>(pc => {

    var logger = pc.GetRequiredService< ILogger<DefaultRabbitMQPersistentConnection>>();
    var factory = new ConnectionFactory()
    {
        HostName = builder.Configuration["EventBus:HostName"]

    };

    if (!string.IsNullOrWhiteSpace(builder.Configuration["EventBus:UserName"]))
    {
        factory.UserName = builder.Configuration["EventBus:UserName"];
    }
    if (!string.IsNullOrWhiteSpace(builder.Configuration["EventBus:Password"]))
    {
        factory.HostName = builder.Configuration["EventBus:Password"];
    }

    var retryCount = 5;
    if (!string.IsNullOrWhiteSpace(builder.Configuration["EventBus:RetryPolicy"]))
    {
        retryCount = int.Parse(builder.Configuration["EventBus:RetryPolicy"]);
    }

    return new DefaultRabbitMQPersistentConnection(factory, retryCount, logger);

});
builder.Services.AddSingleton<EventBusRabbitMQProducer>();

#endregion
var app = builder.Build();



// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

