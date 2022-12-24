using System;
using ESourcing.Products.Entities;
using MongoDB.Driver;

namespace ESourcing.Products.Data.Abstract
{
	public interface IProductContext
	{
        IMongoCollection<Product> Products { get; }
    }

}

