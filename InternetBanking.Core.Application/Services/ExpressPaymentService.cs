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
using System.Transactions;

namespace InternetBanking.Core.Application.Services
{
    public class ExpressPaymentService : IExpressService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        public ExpressPaymentService(IPaymentRepository paymentRepository, IMapper mapper, ITransactionService transactionService)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _transactionService = transactionService;
        }

        public async Task<PaymentViewModel> ProcessPaymentData(SavePaymentViewModel vm)
        {
            var OriginProduct = await ((IPaymentService)this).GetProductWithIncludes(vm.OriginProduct, _paymentRepository);
            if (OriginProduct == null)
            {
                throw new KeyNotFoundException("No se encontró la cuenta de origen");
            }

            if (OriginProduct.Balance < vm.Amount)
            {
                throw new InvalidOperationException("Esta cuenta no cuenta con fondos suficientes para completar el pago");
            }

            var DestinationProduct = await ((IPaymentService)this).GetProductWithIncludes(vm.DestinationProduct, _paymentRepository);

            if (DestinationProduct == null)
            {
                throw new InvalidOperationException("No se ha encontrado la cuenta destino");
            }

            var paymentConfirmation = _mapper.Map<PaymentViewModel>(DestinationProduct);

            paymentConfirmation.Amount = vm.Amount;
            paymentConfirmation.OriginProduct = OriginProduct;
            paymentConfirmation.PaymentType = vm.PaymentType;
            return paymentConfirmation;
        }

        public async Task ConfirmPayment(PaymentViewModel vm)
        {

           

            var originProduct = await ((IPaymentService)this).GetProductWithIncludes(vm.OriginProduct.ProductNumber, _paymentRepository);
            var destinationProduct = await ((IPaymentService)this).GetProductWithIncludes(vm.DestinationProductNumber, _paymentRepository);

           
            if (originProduct == null || destinationProduct == null)
            {
                throw new InvalidOperationException("Ocurrio un error inesperado");
            }

            originProduct.Balance -= vm.Amount;
            destinationProduct.Balance += vm.Amount;

            SaveTransactionViewModel tvm = new SaveTransactionViewModel
            {
                UserID = originProduct.UserID,
                ProductID = originProduct.Id,
                TransactionTypeID = 1,
                Amount = vm.Amount,
                TransactionDate = DateTime.Now,
            };

            await _transactionService.Add(tvm);
            await _paymentRepository.Update(originProduct, originProduct.Id);
            await _paymentRepository.Update(destinationProduct, destinationProduct.Id);

        }

        //public async Task<Product> GetProductWithIncludes(string pNumber)
        //{
        //    var product = await _paymentRepository.GetByNumberWithIncludesAsync(pNumber, new List<string> { "User", "productType" });
        //    return product;
        //}


    }
}
