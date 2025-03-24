//using AutoMapper;
//using InternetBanking.Core.Application.Interfaces.Repositories;
//using InternetBanking.Core.Application.Interfaces.Services;
//using InternetBanking.Core.Application.ViewModels.PayementVMS;
//using InternetBanking.Core.Application.ViewModels.TransactionVMS;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace InternetBanking.Core.Application.Services
//{
//    public class BeneficiaryPaymentService : IBeneficiaryService
//    {
//        private readonly IPaymentRepository _paymentRepository;
//        private readonly ITransactionService _transactionService;
//        private readonly IMapper _mapper;

//        public BeneficiaryPaymentService(IPaymentRepository paymentRepository, IMapper mapper, ITransactionService transactionService)
//        {
//            _paymentRepository = paymentRepository;
//            _mapper = mapper;
//            _transactionService = transactionService;
//        }

//        public async Task<PaymentViewModel> ProcessPaymentData(SavePaymentViewModel vm)
//        {
//            var originProduct = await ((IPaymentService)this).GetProductWithIncludes(vm.OriginProduct, _paymentRepository);
//            var BeneficiaryProduct = await ((IPaymentService)this).GetProductWithIncludes(vm.DestinationProduct, _paymentRepository);

//            if(originProduct == null || BeneficiaryProduct == null)
//            {
//                throw new InvalidOperationException("Error en los datos ingresados, reviselos e intentelo de nuevo.");
//            }

//            if (originProduct.Balance < vm.Amount)
//            {
//                throw new InvalidOperationException("La cuenta origen no cuenta con suficientes fondos.");
//            }

//            var confirViewModel = _mapper.Map<PaymentViewModel>(BeneficiaryProduct);
//            confirViewModel.Amount = vm.Amount;
//            confirViewModel.OriginProduct = originProduct;
//            confirViewModel.PaymentType = vm.PaymentType;
//            return confirViewModel;
//        }

//        public async  Task ConfirmPayment(PaymentViewModel vm)
//        {
//            var originProduct = await((IPaymentService)this).GetProductWithIncludes(vm.OriginProduct.ProductNumber, _paymentRepository);
//            var BeneficiaryProduct = await((IPaymentService)this).GetProductWithIncludes(vm.DestinationProductNumber, _paymentRepository);

//            if (originProduct == null || BeneficiaryProduct == null)
//            {
//                throw new InvalidOperationException("Ocurrio un error inesperado");
//            }

//            originProduct.Balance -= vm.Amount;
//            BeneficiaryProduct.Balance += vm.Amount;

//            SaveTransactionViewModel tvm = new SaveTransactionViewModel
//            {
//                UserID = originProduct.UserID,
//                ProductID = originProduct.Id,
//                TransactionTypeID = 5,
//                Amount = vm.Amount,
//                TransactionDate = DateTime.Now,
//            };

//            await _transactionService.Add(tvm);
//            await _paymentRepository.Update(originProduct, originProduct.Id);
//            await _paymentRepository.Update(BeneficiaryProduct, BeneficiaryProduct.Id);
//        }
//    }
//}
