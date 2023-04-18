using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ProductService.Dtos;
using RabbitMQ.Client;

namespace ProductService.AsyncDataService
{
    public class MessageClient : IMessageClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger_product", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("--> Connected to Message Broker");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not connect to RabbitMQ: {ex.Message}");
            }
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }

        public void PublishNewProduct(ProductPublishDto productPublishDto)
        {
            var message = JsonSerializer.Serialize(productPublishDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ connection is open, sending message...");
                SendMessage(message);
                Console.WriteLine($"{message}");
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connection is closed, not sending...");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger_product", routingKey: "",
            basicProperties: null, body: body);
            Console.WriteLine($"--> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("--> Message Bus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}