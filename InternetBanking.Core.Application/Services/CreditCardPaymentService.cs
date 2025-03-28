using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.PayementVMS;
using InternetBanking.Core.Application.ViewModels.TransactionVMS;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class CreditCardPaymentService: ICreditCardService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITransactionService _transactionservice;
        public readonly IMapper _mapper;

        public CreditCardPaymentService(IPaymentRepository paymentRepository, IMapper mapper, ITransactionService transactionservice)
        {
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _transactionservice = transactionservice;
        }

        public async Task<PaymentViewModel> ProcessPaymentData(SavePaymentViewModel vm)
        {
            var CreditCard = await ((IPaymentService)this).GetProductWithIncludes(vm.DestinationProduct, _paymentRepository);
            var OriginProduct = await ((IPaymentService)this).GetProductWithIncludes(vm.OriginProduct, _paymentRepository);

            if (CreditCard==null|| OriginProduct==null) 
            {
                throw new InvalidOperationException("Error en los datos ingresados, reviselos e intentelo de nuevo.");
            }

            if(OriginProduct.Balance < vm.Amount)
            {
                throw new InvalidOperationException("La cuenta origen no cuenta con suficientes fondos.");
            }

            var ConfirmViewModel = _mapper.Map<PaymentViewModel>(CreditCard);
            ConfirmViewModel.Amount = vm.Amount;
            ConfirmViewModel.OriginProduct = OriginProduct;
            ConfirmViewModel.PaymentType = vm.PaymentType;
            return ConfirmViewModel;
        }

        public async Task ConfirmPayment(PaymentViewModel vm)
        {
            var OriginProduct = await ((IPaymentService)this).GetProductWithIncludes(vm.OriginProduct.ProductNumber, _paymentRepository);
            var CreditCard = await ((IPaymentService)this).GetProductWithIncludes(vm.DestinationProductNumber, _paymentRepository);

            if(CreditCard.Balance == 0)
            {
                throw new InvalidOperationException("Esta tarjeta ya está pagada");
            }

            if (vm.Amount > CreditCard.Balance)
            {
                var rest = vm.Amount - CreditCard.Balance;
                OriginProduct.Balance -= vm.Amount;
                OriginProduct.Balance += rest;

                vm.Amount = CreditCard.Balance;
                CreditCard.Balance = 0;

            }
            else
            {
                CreditCard.Balance -= vm.Amount;
                OriginProduct.Balance -= vm.Amount;
            }

            SaveTransactionViewModel tvm = new SaveTransactionViewModel
            {
                UserID = OriginProduct.UserID,
                ProductID = OriginProduct.Id,
                TransactionTypeID = 2,
                Amount = vm.Amount,
                TransactionDate = DateTime.Now,
            };

            await _transactionservice.Add(tvm);
            await _paymentRepository.Update(OriginProduct, OriginProduct.Id);
            await _paymentRepository.Update(CreditCard, CreditCard.Id);

        }

        //public async Task<Product> GetProductWithIncludes(string pNumber)
        //{
        //    var product = await _paymentRepository.GetByNumberWithIncludesAsync(pNumber, new List<string> { "User", "productType" });
        //    return product;
        //}
    }
}
