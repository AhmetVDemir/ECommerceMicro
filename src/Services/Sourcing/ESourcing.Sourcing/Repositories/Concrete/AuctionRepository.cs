using System;
using System.Reflection.Metadata.Ecma335;
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

        public async Task<Auction> GetAuction(string id)
        {
            return await _context.Auctions.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Auction> GetAuctionByName(string name)
        {
            FilterDefinition<Auction> filter = Builders<Auction>.Filter.Eq(n => n.Name, name);
            return await _context.Auctions.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Auction>> GetAuctions()
        {
            return await _context.Auctions.Find(a => true).ToListAsync();
        }

        public async Task<bool> Update(Auction auction)
        {
            var updateResult = await _context.Auctions.ReplaceOneAsync(a => a.Id.Equals(auction.Id), auction);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        #endregion
    }
}

