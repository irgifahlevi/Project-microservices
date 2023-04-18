using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private readonly ILogger<MessageBusSubscriber> _logger;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;
        private string _productQueueName;
        private string _walletQueueName;
        private string _walletTopupQueueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor,
            ILogger<MessageBusSubscriber> logger)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            _logger = logger;

            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            // _connection = factory.CreateConnection();
            // _channel = _connection.CreateModel();
            // _channel.ExchangeDeclare(exchange: "trigger_product", type: ExchangeType.Fanout);
            // _queueName = _channel.QueueDeclare().QueueName;
            // _channel.QueueBind(queue: _queueName, exchange: "trigger_product", routingKey: "");
            // _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare product
            _channel.ExchangeDeclare(exchange: "trigger_product", type: ExchangeType.Fanout);
            _productQueueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _productQueueName, exchange: "trigger_product", routingKey: "");

            // Declare wallet
            _channel.ExchangeDeclare(exchange: "trigger_wallet", type: ExchangeType.Fanout);
            _walletQueueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _walletQueueName, exchange: "trigger_wallet", routingKey: "");

            // Declare topup wallet
            _channel.ExchangeDeclare(exchange: "trigger_topup_wallet", type: ExchangeType.Fanout);
            _walletTopupQueueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _walletTopupQueueName, exchange: "trigger_topup_wallet", routingKey: "");

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            //Console.WriteLine("RabbitMQ Connection Shutdown");
            _logger.LogInformation("RabbitMQ Connection Shutdown");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            // var consumer = new EventingBasicConsumer(_channel);
            // consumer.Received += (ModuleHandle, ea) =>
            // {
            //     Console.WriteLine("--> Event Received !"); var body = ea.Body;
            //     var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
            //     _eventProcessor.ProccessEvent(notificationMessage);
            // };
            // _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            // return Task.CompletedTask;

            var productConsumer = new EventingBasicConsumer(_channel);
            productConsumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Product Event Received !");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.ProccessEvent(notificationMessage);
            };
            _channel.BasicConsume(queue: _productQueueName, autoAck: true, consumer: productConsumer);

            // Register a consumer for the wallet trigger
            var walletConsumer = new EventingBasicConsumer(_channel);
            walletConsumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Wallet Event Received !");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.ProccessEvent(notificationMessage);
            };
            _channel.BasicConsume(queue: _walletQueueName, autoAck: true, consumer: walletConsumer);


            var walletTopupConsumer = new EventingBasicConsumer(_channel);
            walletTopupConsumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Wallet Topup Event Received !");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.ProccessEvent(notificationMessage);
            };
            _channel.BasicConsume(queue: _walletTopupQueueName, autoAck: true, consumer: walletTopupConsumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }

    }
}