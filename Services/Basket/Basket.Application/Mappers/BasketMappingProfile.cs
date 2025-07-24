using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Entities;

namespace Basket.Application.Mappers
{
    public class BasketMappingProfile : Profile
    {
        public BasketMappingProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartItemResponse>().ReverseMap();
            CreateMap<ShoppingCartItem, ShoppingCartItemResponse>().ReverseMap();
        }
    }
}
