using ESourcing.Sourcing.Data.Abstract;
using ESourcing.Sourcing.Data.Concrete;
using ESourcing.Sourcing.Settings.Abstract;
using ESourcing.Sourcing.Settings.Concrete;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region MyConfigs
builder.Services.Configure<SourcingDatabaseSettings>(builder.Configuration.GetSection(nameof(SourcingDatabaseSettings)));
builder.Services.AddSingleton<ISourcingDatabaseSettings>(sp=>sp.GetRequiredService<IOptions<SourcingDatabaseSettings>>().Value);
builder.Services.AddTransient<ISourcingContext, SourcingContext>();

#endregion
var app = builder.Build();



// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

