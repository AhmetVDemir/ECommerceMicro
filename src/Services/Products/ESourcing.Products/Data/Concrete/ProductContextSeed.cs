using System;
using ESourcing.Products.Entities;
using MongoDB.Driver;

namespace ESourcing.Products.Data.Concrete
{
	public class ProductContextSeed
	{
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(cc => true).Any();
            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetConfigureProducts());
            }
        }

        private static IEnumerable<Product> GetConfigureProducts()
        {
            return new List<Product>() {

                new Product()
                {
                    Name = "İphone 8",
                    Summary = "Apple telefonu özet",
                    Description = "Açıklamadır",
                    ImageFile = "img1.png",
                    Price = 5000.00M,
                    Category = "Smart Phone"

                },
                 new Product()
                {
                    Name = "Samsun S8",
                    Summary = "Samsung telefonu özet",
                    Description = "Açıklamadır",
                    ImageFile = "img2.png",
                    Price = 8000.00M,
                    Category = "Smart Phone"

                },
                  new Product()
                {
                    Name = "Microsoft Surface 9",
                    Summary = "Microsoft bilgisayar özet",
                    Description = "Açıklamadır",
                    ImageFile = "img3.png",
                    Price = 15000.00M,
                    Category = "Laptop"

                },
            };
        }
    }
}

