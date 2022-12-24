using System;
using ESourcing.Products.Data.Abstract;
using ESourcing.Products.Entities;
using ESourcing.Products.Repositories.Abstract;
using MongoDB.Driver;

namespace ESourcing.Products.Repositories.Concrete
{
	public class ProductRepository : IProductRepository
    {
        private readonly IProductContext _context;

        public ProductRepository(IProductContext context)
        {
            _context = context;
        }

        //-------------------------------------------------------


        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Product>.Filter.Eq(c => c.Id, id);
            DeleteResult deleteResult = await _context.Products.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetProduct()
        {
            return await _context.Products.Find(p => true).ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            var filters = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
            return await _context.Products.Find(filters).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            var filters = Builders<Product>.Filter.ElemMatch(p => p.Name, name);
            return await _context.Products.Find(filters).ToListAsync();
        }

        public async Task<bool> Uppdate(Product product)
        {
            var updateResult = await _context.Products.ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}

