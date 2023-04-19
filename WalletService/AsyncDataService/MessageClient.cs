using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using WalletService.Dtos;

namespace WalletService.AsyncDataService
{
    public class MessageClient : IMessageClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private const string TriggerWalletExchangeName = "trigger_wallet";
        private const string TriggerTopUpWalletExchangeName = "trigger_topup_wallet";

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
                // _channel.ExchangeDeclare(exchange: "trigger_wallet", type: ExchangeType.Fanout);

                // Create exchange for trigger_wallet
                CreateExchange(TriggerWalletExchangeName);

                // Create exchange for trigger_topup_wallet
                CreateExchange(TriggerTopUpWalletExchangeName);
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

        private void CreateExchange(string exchangeName)
        {
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
        }

        public void PublishNewWallet(WalletPublishDto walletPublishDto)
        {
            var message = JsonSerializer.Serialize(walletPublishDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ connection is open, sending message...");
                SendMessage(message, TriggerWalletExchangeName);
                Console.WriteLine($"{message}");
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connection is closed, not sending...");
            }
        }

        public void PublishTopupWallet(TopupWalletPublishDto topupWalletPublishDto)
        {
            var message = JsonSerializer.Serialize(topupWalletPublishDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ connection is open, sending message...");
                SendMessage(message, TriggerTopUpWalletExchangeName);
                Console.WriteLine($"{message}");
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connection is closed, not sending...");
            }
        }

        // private void SendMessage(string message)
        // {
        //     var body = Encoding.UTF8.GetBytes(message);
        //     _channel.BasicPublish(exchange: "trigger_wallet", routingKey: "",
        //     basicProperties: null, body: body);
        //     Console.WriteLine($"--> We have sent {message}");
        // }

        private void SendMessage(string message, string exchangeName)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: exchangeName, routingKey: "",
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