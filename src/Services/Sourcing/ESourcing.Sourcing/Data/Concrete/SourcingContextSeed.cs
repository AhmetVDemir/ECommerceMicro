using System;
using ESourcing.Sourcing.Entities.Concrete;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Data.Concrete
{
	public class SourcingContextSeed
	{
		public static void SeedData(IMongoCollection<Auction> auctionCollection)
		{
			bool exist = auctionCollection.Find(auc => true).Any();
			if (!exist)
			{
				auctionCollection.InsertManyAsync(GetPreconfiguredAuctions());
			}
		}

		private static IEnumerable<Auction> GetPreconfiguredAuctions()
		{
			return new List<Auction>() {
				new Auction()
				{
					Name = "Auction1",
					Description = "Lorem imsum ccccc",
					CreatdAt = DateTime.Now,
					StartedAt = DateTime.Now,
					FinishedAt = DateTime.Now.AddDays(10),
					ProductId = "63ad9b79041a7cffa2337c24",
					IncludedSellers = new List<string>()
					{
						"sellers0@sellers.com",
						"sellers1@sellers.com",
						"sellers3@sellers.com"
					},
					Quantity = 5,
					Status =(int) Status.Active
                },
                new Auction()
                {
                    Name = "Auction2",
                    Description = "Lorem imsum ccccc",
                    CreatdAt = DateTime.Now,
                    StartedAt = DateTime.Now,
                    FinishedAt = DateTime.Now.AddDays(10),
                    ProductId = "63ad9b79041a7cffa2337c24",
                    IncludedSellers = new List<string>()
                    {
                        "sellers0@sellers.com",
                        "sellers1@sellers.com",
                        "sellers3@sellers.com"
                    },
                    Quantity = 6,
                    Status =(int) Status.Active
                },
                new Auction()
                {
                    Name = "Auction3",
                    Description = "Lorem imsum ccccc",
                    CreatdAt = DateTime.Now,
                    StartedAt = DateTime.Now,
                    FinishedAt = DateTime.Now.AddDays(10),
                    ProductId = "63ad9b79041a7cffa2337c24",
                    IncludedSellers = new List<string>()
                    {
                        "sellers0@sellers.com",
                        "sellers1@sellers.com",
                        "sellers3@sellers.com"
                    },
                    Quantity = 7,
                    Status =(int) Status.Active
                },
                new Auction()
                {
                    Name = "Auction4",
                    Description = "Lorem imsum ccccc",
                    CreatdAt = DateTime.Now,
                    StartedAt = DateTime.Now,
                    FinishedAt = DateTime.Now.AddDays(10),
                    ProductId = "63ad9b79041a7cffa2337c24",
                    IncludedSellers = new List<string>()
                    {
                        "sellers0@sellers.com",
                        "sellers1@sellers.com",
                        "sellers3@sellers.com"
                    },
                    Quantity = 8,
                    Status =(int) Status.Active
                }
            };
		}

    }
}

