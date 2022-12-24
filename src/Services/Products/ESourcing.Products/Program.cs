using ESourcing.Products.Data.Abstract;
using ESourcing.Products.Data.Concrete;
using ESourcing.Products.Repositories.Abstract;
using ESourcing.Products.Repositories.Concrete;
using ESourcing.Products.Settings.Abstract;
using ESourcing.Products.Settings.Concrete;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


#region MyConfig

builder.Services.Configure<ProductDatabaseSettings>(builder.Configuration.GetSection(nameof(ProductDatabaseSettings)));
builder.Services.AddSingleton<IProductDatabaseSettings>(prd => prd.GetRequiredService<IOptions<ProductDatabaseSettings>>().Value);
builder.Services.AddSingleton<IProductContext, ProductContext>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();

#endregion


builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

