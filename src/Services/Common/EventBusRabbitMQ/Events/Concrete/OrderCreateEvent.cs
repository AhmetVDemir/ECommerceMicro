using System;
using EventBusRabbitMQ.Events.Abstract;

namespace EventBusRabbitMQ.Events.Concrete
{
	public class OrderCreateEvent : IAEvent
	{
        public string Id { get; set; }

        public string AuctionId { get; set; }

        public string ProductId { get; set; }

        public string SellerUserName { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Quantity { get; set; }

    }
}

