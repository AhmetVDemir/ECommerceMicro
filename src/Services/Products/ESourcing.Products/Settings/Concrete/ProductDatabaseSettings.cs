using System;
using ESourcing.Products.Settings.Abstract;

namespace ESourcing.Products.Settings.Concrete
{
	public class ProductDatabaseSettings: IProductDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}

