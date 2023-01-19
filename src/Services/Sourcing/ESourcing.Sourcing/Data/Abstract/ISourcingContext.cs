using System;
using MongoDB.Driver;
using ESourcing.Sourcing.Entities.Concrete;

namespace ESourcing.Sourcing.Data.Abstract
{
	public interface ISourcingContext
	{
		IMongoCollection<Auction> Auctions { get; }

		IMongoCollection<Bid> Bids { get; }
	}
}

