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
    public class AuctionController : ControllerBase
    {
        #region Variables

        private readonly IAuctionRepository _auctionRepository;
        private readonly ILogger<AuctionController> _logger;
        public AuctionController(IAuctionRepository auctionRepository, ILogger<AuctionController> logger)
        {
            _auctionRepository = auctionRepository;
            _logger = logger;
        }

        #endregion

        #region Endpoints

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Auction>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAuctions()
        {
            var auctions = await _auctionRepository.GetAuctions();
            return Ok(auctions);
        }


        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Auction>> GetAction(string id)
        {
            var auction = await _auctionRepository.GetAuction(id);

            if (auction == null)
            {
                _logger.LogError($"Veritabanında {id} li auction bulunamadı");
                return NotFound();
            }
            return Ok(auction);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Auction>> CreateAuction([FromBody]Auction auction)
        {
             await _auctionRepository.Create(auction);
            return CreatedAtRoute("GetAction", new { id = auction.Id }, auction); 
        }

        [HttpPut]
        [ProducesResponseType(typeof(Auction),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> UpdateAuction([FromBody] Auction auction)
        {
            return Ok(await _auctionRepository.Update(auction));
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> DeleteAuctionById(string id)
        {
            return Ok(await _auctionRepository.Delete(id));
        }


        #endregion
    }
}

