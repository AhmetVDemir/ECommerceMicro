using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using ESourcing.Sourcing.Entities.Concrete;
using ESourcing.Sourcing.Repositories.Abstract;
using EventBusRabbitMQ.Core.Constants;
using EventBusRabbitMQ.Events.Concrete;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        #region Variables

        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly ILogger<AuctionController> _logger;
        private readonly EventBusRabbitMQProducer _eventBus;
        private readonly IMapper _mapper;
        public AuctionController(EventBusRabbitMQProducer eventBus, IMapper mapper,IAuctionRepository auctionRepository,IBidRepository bidRepository , ILogger<AuctionController> logger)
        {
            _eventBus = eventBus;
            _auctionRepository = auctionRepository;
            _logger = logger;
            _bidRepository = bidRepository;
            _mapper = mapper;
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


        [HttpPost("ComplateAuction")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<ActionResult> ComplateAuction(string id)
        {
            Auction auction = await _auctionRepository.GetAuction(id);
            if(auction == null)
                return NotFound();
            if(auction.Status != (int)Status.Active)
            {
                _logger.LogError("Mezat tamamlanmadı");
                return BadRequest();
            }

            Bid bid = await _bidRepository.GetWinnerBid(id);
            if (bid == null)
            {
                return NotFound();
            }

            OrderCreateEvent eventMessage = _mapper.Map<OrderCreateEvent>(bid);
            eventMessage.Quantity = auction.Quantity;

            auction.Status = (int)Status.Closed;

            bool updateResponse = await _auctionRepository.Update(auction);
            if (!updateResponse)
            {
                _logger.LogError("Metat güncellenemedi.");
                return BadRequest();
            }


            try
            {
                _eventBus.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hata : evet : {EventId} uygulama {AppName}", eventMessage.Id, "Sourcing");
                throw;
            }
            return Accepted();
        }


        [HttpPost("TestEvent")]
        public ActionResult<OrderCreateEvent> TestEvent()
        {

            OrderCreateEvent eventMessage = new OrderCreateEvent();
            eventMessage.AuctionId = "dummy1";
            eventMessage.ProductId = "dummy_product_1";
            eventMessage.Price = 10;
            eventMessage.Quantity = 100;
            eventMessage.SellerUserName = "deneme@deneme.com";

            try
            {
                _eventBus.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hata ! {EventId} {AppName} ", eventMessage.Id, "Sourcing");
                throw;
            }
            return Accepted(eventMessage);
        }

        #endregion
    }
}

