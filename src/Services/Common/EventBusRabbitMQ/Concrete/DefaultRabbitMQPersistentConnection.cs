using System;
using System.Net.Sockets;
using EventBusRabbitMQ.Abstract;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBusRabbitMQ.Concrete
{
    public class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {

        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private readonly int _retryCount;
        private readonly ILogger<DefaultRabbitMQPersistentConnection> _logger;
        private bool _disposed;

        public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount, ILogger<DefaultRabbitMQPersistentConnection> logger)
        {
            _connectionFactory = connectionFactory;
            _retryCount = retryCount;
            _logger = logger;
        }

        public bool isConnected { get { return _connection != null && _connection.IsOpen && !_disposed; } }

        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ Client bağlanmayı deniyor !");

            var policy = RetryPolicy.Handle<SocketException>().Or<BrokerUnreachableException>().WaitAndRetry(_retryCount,retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                _logger.LogWarning(ex, "RabbitMQ Client cold not connection after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex);

            });

            policy.Execute(() =>
            {
                _connection = _connectionFactory.CreateConnection();
            });
            if (isConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                _logger.LogInformation("RabbitMQ Client!");
                return true;

            }
            else
            {
                _logger.LogCritical("Fatal Error : RabbitMQ Client bağlantısı oluşuturalamdı");
                return false;
            }
        }

        public void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;
            _logger.LogWarning("Bir rabbitmq bağlantısı kapatıldı tekrar deneniyor....");
            TryConnect();
        }
        public void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;
            _logger.LogWarning("Bir rabbitmq bağlantısı hata verdi tekrar deneniyor....");
            TryConnect();
        }
        public void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;
            _logger.LogWarning("Bir rabbitmq bağlantısı kapatıldı tekrar deneniyor....");
            TryConnect();
        }


        public IModel CreateModel()
        {
            if (!isConnected)
            {
                throw new InvalidOperationException("u aksiyon için gerekli bir rabbitmq bağlantısı yok");
            }
            return _connection.CreateModel();
        }

       

       
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }
    }
}

