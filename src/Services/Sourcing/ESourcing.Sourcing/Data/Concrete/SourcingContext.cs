using System;
using ESourcing.Sourcing.Data.Abstract;
using ESourcing.Sourcing.Entities.Concrete;
using ESourcing.Sourcing.Settings.Abstract;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Data.Concrete
{
    public class SourcingContext : ISourcingContext
    {

        public SourcingContext(ISourcingDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Auctions = database.GetCollection<Auction>(nameof(Auction));
            Bids = database.GetCollection<Bid>(nameof(Bid));
        }

        public IMongoCollection<Auction> Auctions { get; }

        public IMongoCollection<Bid> Bids { get; }
    }
}

