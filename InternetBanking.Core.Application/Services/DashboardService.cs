using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.DasboardVMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    class DashboardService : IDashboardService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;

        public DashboardService(ITransactionRepository transactionRepository, IProductRepository productRepository)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
        }


        public async Task<DashboardViewModel> DashboardInfo()
        {
            var transactions = await _transactionRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();
            var payments = await _transactionRepository.GetAllAsync();

            var paymentsfiltred = payments.Where(p => p.TransactionTypeID != 4);
            var todaytransaction = transactions.Where(t => t.TransactionDate.Date == DateTime.Today);
            var todaypayments = paymentsfiltred.Where(t => t.TransactionDate.Date == DateTime.Today);

            DashboardViewModel dvm = new DashboardViewModel
            {
                products = products.Count(),
                transactions = transactions.Count(),
                payments = paymentsfiltred.Count(),
                todayTransactions = todaytransaction.Count(),
                todayPayments = todaypayments.Count(),
            };
            return dvm;
        }
    }
}
