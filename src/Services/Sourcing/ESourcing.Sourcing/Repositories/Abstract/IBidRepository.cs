using System;
using ESourcing.Sourcing.Entities.Concrete;

namespace ESourcing.Sourcing.Repositories.Abstract
{
	public interface IBidRepository
	{
		Task SendBid(Bid bid);
		Task<List<Bid>> GetBidsByAuctionId(string id);
		Task<Bid> GetWinnerBid(string id);
	}
}

