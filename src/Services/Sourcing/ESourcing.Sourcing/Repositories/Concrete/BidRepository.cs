using System;
using ESourcing.Sourcing.Data.Abstract;
using ESourcing.Sourcing.Entities.Concrete;
using ESourcing.Sourcing.Repositories.Abstract;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Repositories.Concrete
{
    public class BidRepository : IBidRepository
    {

        #region Variables
        private readonly ISourcingContext _context;
        public BidRepository(ISourcingContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<List<Bid>> GetBidsByAuctionId(string id)
        {
            FilterDefinition<Bid> filter = Builders<Bid>.Filter.Eq(a => a.AuctionId, id);
            List<Bid> bids = await _context.Bids.Find(filter).ToListAsync();
            bids = bids.OrderByDescending(a => a.CreatedAt).GroupBy(a => a.SellerUserName).Select(a => new Bid
            {
                AuctionId = a.FirstOrDefault().AuctionId,
                Price = a.FirstOrDefault().Price,
                CreatedAt = a.FirstOrDefault().CreatedAt,
                SellerUserName = a.FirstOrDefault().SellerUserName,
                ProductId = a.FirstOrDefault().ProductId,
                Id = a.FirstOrDefault().Id
            }).ToList();

            return bids;
        }

        public async Task<Bid> GetWinnerBid(string id)
        {
            List<Bid> bids = await GetBidsByAuctionId(id);
            return bids.OrderByDescending(a => a.Price).FirstOrDefault();

        }

        public async Task SendBid(Bid bid)
        {
            await _context.Bids.InsertOneAsync(bid);
        }
    }
}

