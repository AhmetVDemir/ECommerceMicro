using System;
using RabbitMQ.Client;

namespace EventBusRabbitMQ.Abstract
{
	public interface IRabbitMQPersistentConnection : IDisposable
	{
		public bool isConnected { get; }

		bool TryConnect();

		IModel CreateModel();
	}
}

