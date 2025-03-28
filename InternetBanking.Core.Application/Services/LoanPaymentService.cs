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
    public class LoanPaymentService: ILoanService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITransactionService _transactionService;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public LoanPaymentService(IPaymentRepository paymentRepository, IMapper mapper, ITransactionService transactionService, IProductRepository productRepository)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _transactionService = transactionService;
            _productRepository = productRepository;
        }
        public async Task<PaymentViewModel> ProcessPaymentData(SavePaymentViewModel vm)
        {
            var originproduct = await ((IPaymentService)this).GetProductWithIncludes(vm.OriginProduct, _paymentRepository);
            var loan = await ((IPaymentService)this).GetProductWithIncludes(vm.DestinationProduct, _paymentRepository);

            if (loan == null || originproduct == null)
            {
                throw new InvalidOperationException("Error en los datos ingresados, reviselos e intentelo de nuevo.");
            }

            if (originproduct.Balance < vm.Amount)
            {
                throw new InvalidOperationException("La cuenta origen no cuenta con suficientes fondos.");
            }

            var confirmViewModel = _mapper.Map<PaymentViewModel>(loan);
            confirmViewModel.Amount = vm.Amount;
            confirmViewModel.PaymentType = vm.PaymentType;
            confirmViewModel.OriginProduct = originproduct;
            return confirmViewModel;

        }
        public async Task ConfirmPayment(PaymentViewModel vm)
        {
            var originproduct = await((IPaymentService)this).GetProductWithIncludes(vm.OriginProduct.ProductNumber, _paymentRepository);
            var loan = await((IPaymentService)this).GetProductWithIncludes(vm.DestinationProductNumber, _paymentRepository);

            if (loan.Balance == 0)
            {
                throw new InvalidOperationException("Este prestamo ya está pago");
            }

            if (vm.Amount > loan.Balance)
            {
                var rest = vm.Amount - loan.Balance;
                originproduct.Balance -= vm.Amount;
                originproduct.Balance += rest;

                vm.Amount = loan.Balance;
                loan.Balance = 0;
            }
            else
            {
                loan.Balance -= vm.Amount;
                originproduct.Balance -= vm.Amount;
            }

            SaveTransactionViewModel tvm = new SaveTransactionViewModel
            {
                UserID = originproduct.UserID,
                ProductID = originproduct.Id,
                TransactionTypeID = 3,
                Amount = vm.Amount,
                TransactionDate = DateTime.Now,
            };

            await _transactionService.Add(tvm);
            await _paymentRepository.Update(originproduct, originproduct.Id);
            var updatedLoan = await _paymentRepository.Update(loan, loan.Id);

            //if (updatedLoan.Balance == 0)
            //{
            //    await _productRepository.DeleteAsync(updatedLoan);
            //}
        }
    }
}
