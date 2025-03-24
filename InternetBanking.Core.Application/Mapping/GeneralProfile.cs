using AutoMapper;
using InternetBanking.Core.Application.ViewModels.PayementVMS;
using InternetBanking.Core.Application.ViewModels.ProductVMS;
using InternetBanking.Core.Application.ViewModels.TransactionVMS;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {

            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.productType, opt => opt.MapFrom(src => src.productType))
                .ForMember(dest => dest.transactions, opt => opt.MapFrom(src => src.transactions));


            CreateMap<Product, PaymentViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DestinationProductNumber, opt => opt.MapFrom(src => src.ProductNumber))
                .ForMember(dest => dest.ProductTypeID, opt => opt.MapFrom(src => src.ProductTypeID))
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.productType, opt => opt.MapFrom(src => src.productType))
                .ForMember(dest => dest.Amount, opt => opt.Ignore())
                .ForMember(dest => dest.OriginProduct, opt => opt.Ignore());

            CreateMap<SaveTransactionViewModel, Transaction>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.transactionType, opt => opt.Ignore())
                .ReverseMap();
                
        
        }
    }
}
