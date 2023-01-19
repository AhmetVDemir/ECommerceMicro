using System;
using ESourcing.Sourcing.Data.Abstract;
using ESourcing.Sourcing.Entities.Concrete;
using ESourcing.Sourcing.Repositories.Abstract;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Repositories.Concrete
{
    public class AuctionRepository : IAuctionRepository
    {

        #region CTOR

        private readonly ISourcingContext _context;

        public AuctionRepository(ISourcingContext context)
        {
            _context = context;
        }

        #endregion

        #region METHODS

        public async Task Create(Auction auction)
        {
            await _context.Auctions.InsertOneAsync(auction);
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Auction> filter = Builders<Auction>.Filter.Eq(m => m.Id ,id);
            DeleteResult deleteResult = await _context.Auctions.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public Task<Auction> GetAuction(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Auction> GetAuctionByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Auction>> GetAuctions()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Auction auction)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

