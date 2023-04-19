using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using OrderService.Data;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _scopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        public void ProccessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.ProductPublished:
                    addProduct(message);
                    break;
                case EventType.WalletPublished:
                    addWallet(message);
                    break;
                case EventType.TopupWalletPublished:
                    topupWallet(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch (eventType.Event)
            {
                case "Product_NewPublished":
                    Console.WriteLine("--> Product_NewPublished Event Detected");
                    return EventType.ProductPublished;
                case "Wallet_NewPublished":
                    Console.WriteLine("--> Wallet_NewPublished Event Detected");
                    return EventType.WalletPublished;
                case "TopupWallet_NewPublished":
                    Console.WriteLine("--> TopupWallet_NewPublished Event Detected");
                    return EventType.TopupWalletPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private void addProduct(string productPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IProductOrderRepo>();
                var productPublishedDto = JsonSerializer.Deserialize<ProductPublishedDto>(productPublishedMessage);
                try
                {
                    var product = _mapper.Map<Product>(productPublishedDto);
                    if (!repo.ExternalProductExists(product.ProductId))
                    {
                        repo.CreateProduct(product);
                        repo.SaveChanges();
                        Console.WriteLine($"--> Product {product.Name} added");
                    }
                    else
                    {
                        Console.WriteLine("--> Product already exists");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Product to DB: {ex.Message}");
                }
            }
        }

        private void addWallet(string walletPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IProductOrderRepo>();
                var walletPublishedDto = JsonSerializer.Deserialize<WalletPublishDto>(walletPublishedMessage);
                try
                {
                    var wallet = _mapper.Map<Wallet>(walletPublishedDto);
                    if (!repo.ExternalWalletExists(wallet.WalletId))
                    {
                        // repo.CreateProduct(product);
                        repo.CreateWallet(wallet);
                        repo.SaveChanges();
                        Console.WriteLine($"--> Wallet {wallet.Username} added");
                    }
                    else
                    {
                        // Console.WriteLine("--> Product already exists");
                        repo.AddCash(wallet.WalletId, walletPublishedDto.Cash);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Product to DB: {ex.Message}");
                }
            }
        }

        private void topupWallet(string topupWalletMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IProductOrderRepo>();
                var walletPublishedDto = JsonSerializer.Deserialize<TopupWalletPublishDto>(topupWalletMessage);
                try
                {
                    var wallet = _mapper.Map<Wallet>(walletPublishedDto);
                    if (!repo.WalletExists(wallet.WalletId))
                    {
                        Console.WriteLine("--> Wallet doesn't exist in database");
                    }
                    else
                    {
                        repo.TopupCash(wallet.WalletId, walletPublishedDto.Cash);
                        Console.WriteLine("--> Topup cash added to wallet order");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Product to DB: {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        ProductPublished,
        WalletPublished,
        TopupWalletPublished,
        Undetermined
    }
}