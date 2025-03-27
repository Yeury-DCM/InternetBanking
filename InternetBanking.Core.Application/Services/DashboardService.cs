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
                productsCount = products.Count(),
                transactionsCount = transactions.Count(),
                paymentsCount = paymentsfiltred.Count(),
                todayTransactionsCount = todaytransaction.Count(),
                todayPaymentsCount = todaypayments.Count(),
            };
            return dvm;
        }

        public async Task<DashboardViewModel> GetUserProductsInfo()
        {
            var products = await _productRepository.GetAllWithIncludesAsync(new List<string> { "productType" });
            var transactions = (await _transactionRepository.GetAllWithIncludesAsync(new List<string> { "transactionType" })).OrderByDescending(t=> t.TransactionDate).ToList();

            DashboardViewModel dvm = new DashboardViewModel
            {
                products = products,
                transactions = transactions
            };
            return dvm;
        }
    }
}
