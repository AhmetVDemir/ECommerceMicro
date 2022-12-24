using System;
namespace ESourcing.Products.Settings.Abstract
{
	public interface IProductDatabaseSettings
	{
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}

