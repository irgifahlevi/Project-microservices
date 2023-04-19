using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Profiles
{
    public class OrderProfiles : Profile
    {
        public OrderProfiles()
        {
            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductPublishedDto, Product>();

            // Untuk wallet
            CreateMap<Wallet, ReadWalletDto>();
            CreateMap<WalletPublishDto, Wallet>();
            CreateMap<TopupWalletPublishDto, Wallet>();

            // Untuk order
            CreateMap<CreateOrderDto, Order>();
            CreateMap<Order, OrderReadDto>();
        }
    }
}