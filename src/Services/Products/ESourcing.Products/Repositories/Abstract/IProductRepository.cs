using System;
using ESourcing.Products.Entities;

namespace ESourcing.Products.Repositories.Abstract
{
	public interface IProductRepository
	{
        Task<IEnumerable<Product>> GetProduct();
        Task<Product> GetProduct(string id);
        Task<IEnumerable<Product>> GetProductByName(string name);
        Task<IEnumerable<Product>> GetProductByCategory(string categoryName);
        Task Create(Product product);
        Task<bool> Uppdate(Product product);
        Task<bool> Delete(string id);
    }
}

