using System;
namespace EventBusRabbitMQ.Events.Abstract
{
	public abstract class IAEvent
	{
		public Guid RequestId { get; private init; }

		public DateTime CreationDate { get; private init; }

		public IAEvent()
		{
			RequestId = Guid.NewGuid();
			CreationDate = DateTime.UtcNow;
		}
	}
}

