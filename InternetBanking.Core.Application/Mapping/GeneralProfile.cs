using AutoMapper;

using InternetBanking.Core.Application.Dtos;

using InternetBanking.Core.Application.ViewModels.AdvanceCashVMS;
using InternetBanking.Core.Application.ViewModels.BeneficiaryVMS;

using InternetBanking.Core.Application.ViewModels.PayementVMS;
using InternetBanking.Core.Application.ViewModels.ProductVMS;
using InternetBanking.Core.Application.ViewModels.TransactionVMS;
using InternetBanking.Core.Application.ViewModels.UserVMS;
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
                .ForMember(dest => dest.productType, opt => opt.MapFrom(src => src.productType))
                .ForMember(dest => dest.transactions, opt => opt.MapFrom(src => src.transactions));

            CreateMap<PaymentViewModel, SavePaymentViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OriginProduct, opt => opt.MapFrom(src => src.OriginProduct.ProductNumber))
                .ForMember(dest => dest.DestinationProduct, opt => opt.MapFrom(src => src.DestinationProductNumber))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType));


            CreateMap<Product, PaymentViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DestinationProductNumber, opt => opt.MapFrom(src => src.ProductNumber))
                .ForMember(dest => dest.ProductTypeID, opt => opt.MapFrom(src => src.ProductTypeID))
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.productType, opt => opt.MapFrom(src => src.productType))
                .ForMember(dest => dest.Amount, opt => opt.Ignore())
                .ForMember(dest => dest.OriginProduct, opt => opt.Ignore());

            CreateMap<SaveTransactionViewModel, Transaction>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.transactionType, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<SaveUserViewModel, SaveUserResponse>()
                .ReverseMap();

            CreateMap<SaveUserViewModel, SaveUserRequest>()
               .ReverseMap();

            // Beneficiary to BeneficiaryViewModel
            CreateMap<Beneficiary, BeneficiaryViewModel>()
                .ForMember(dest => dest.BeneficiaryFullName, opt => opt.MapFrom(src => $"{src.user.Name} {src.user.LastName}"))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.account.ProductNumber));

            // SaveBeneficiaryViewModel to Beneficiary
            CreateMap<SaveBeneficiaryViewModel, Beneficiary>();

            // AdvanceCashViewModel mappings
            CreateMap<Product, AdvanceCashViewModel>()
                .ForMember(dest => dest.CreditCardId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SavingsAccountId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Amount, opt => opt.Ignore())
                .ForMember(dest => dest.InterestRate, opt => opt.Ignore());


        }
    }
}
