using System;
using ESourcing.Products.Data.Abstract;
using ESourcing.Products.Entities;
using ESourcing.Products.Settings.Concrete;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ESourcing.Products.Data.Concrete
{
	public class ProductContext : IProductContext
	{
        public ProductContext(IOptions<ProductDatabaseSettings> options)
        {

            MongoClient client = new MongoClient(options.Value.ConnectionString);
            var dataBase = client.GetDatabase(options.Value.DatabaseName);
            Products = dataBase.GetCollection<Product>(options.Value.CollectionName);

            ProductContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }

    }
}

