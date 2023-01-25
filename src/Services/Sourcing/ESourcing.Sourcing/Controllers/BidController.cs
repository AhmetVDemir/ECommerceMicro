using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ESourcing.Sourcing.Entities.Concrete;
using ESourcing.Sourcing.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;



namespace ESourcing.Sourcing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {

        #region Variables

        private readonly IBidRepository _bidRepository;
        private readonly ILogger _logger;

        public BidController(IBidRepository bidRepository, ILogger logger)
        {
            _bidRepository = bidRepository;
            _logger = logger;
        }

        #endregion

        #region Endpoints

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Bid>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult> SendBid([FromBody]Bid bid)
        {
            await _bidRepository.SendBid(bid);
            return Ok();
        }

        [HttpGet("GetBidsByAuctionId")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidsByAuctionId(string id)
        {
            IEnumerable<Bid> bids = await _bidRepository.GetBidsByAuctionId(id);
            return Ok(bids);
        }

        [HttpGet("GetWinnerBid")]
        [ProducesResponseType(typeof(Bid),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Bid>> GetWinnerBid(string id)
        {
            Bid bid = await _bidRepository.GetWinnerBid(id);
            return Ok(bid);
        }

        #endregion

    }
}

