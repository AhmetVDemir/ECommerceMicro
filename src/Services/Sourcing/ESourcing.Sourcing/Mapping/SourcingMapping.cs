using System;
using AutoMapper;
using ESourcing.Sourcing.Entities.Concrete;
using EventBusRabbitMQ.Events.Concrete;

namespace ESourcing.Sourcing.Mapping
{
	public class SourcingMapping : Profile
	{
		public SourcingMapping()
		{
			CreateMap<OrderCreateEvent, Bid>().ReverseMap();
		}
		
	}
}

