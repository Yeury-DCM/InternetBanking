using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.PayementVMS;
using InternetBanking.Core.Application.ViewModels.TransactionVMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITransactionService _transactionservice;
        public readonly IMapper _mapper;

        public TransferService(IPaymentRepository paymentRepository, ITransactionService transactionservice, IMapper mapper)
        {
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _transactionservice = transactionservice;
        }
        public async Task<PaymentViewModel> ProcessPaymentData(SavePaymentViewModel vm)
        {
            var OriginProduct = await((IPaymentService)this).GetProductWithIncludes(vm.OriginProduct, _paymentRepository);
            if (OriginProduct == null)
            {
                throw new InvalidOperationException("No se encontró la cuenta de origen");
            }

            if (OriginProduct.Balance < vm.Amount)
            {
                throw new InvalidOperationException("Esta cuenta no cuenta con fondos suficientes para completar el pago");
            }

            var DestinationProduct = await((IPaymentService)this).GetProductWithIncludes(vm.DestinationProduct, _paymentRepository);

            if (DestinationProduct == null)
            {
                throw new InvalidOperationException("No se ha encontrado la cuenta destino");
            }

            if (OriginProduct.ProductNumber == DestinationProduct.ProductNumber)
            {
                throw new InvalidOperationException($"No se puede transferir dinero a la misma cuenta ({OriginProduct.ProductNumber}).");
            }

            var paymentConfirmation = _mapper.Map<PaymentViewModel>(DestinationProduct);

            paymentConfirmation.Amount = vm.Amount;
            paymentConfirmation.OriginProduct = OriginProduct;
            paymentConfirmation.PaymentType = vm.PaymentType;
            return paymentConfirmation;
        }
        public async Task ConfirmPayment(PaymentViewModel vm)
        {
            var originProduct = await((IPaymentService)this).GetProductWithIncludes(vm.OriginProduct.ProductNumber, _paymentRepository);
            var destinationProduct = await((IPaymentService)this).GetProductWithIncludes(vm.DestinationProductNumber, _paymentRepository);


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
                TransactionTypeID = 4,
                Amount = vm.Amount,
                TransactionDate = DateTime.Now,
            };

            await _transactionservice.Add(tvm);
            await _paymentRepository.Update(originProduct, originProduct.Id);
            await _paymentRepository.Update(destinationProduct, destinationProduct.Id);

        }

    }
}
