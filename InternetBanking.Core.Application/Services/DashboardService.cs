using InternetBanking.Core.Application.Dtos;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.DasboardVMS;
using InternetBanking.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

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
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AuthenticationResponse _userAutneticated;

        public DashboardService(ITransactionRepository transactionRepository, IProductRepository productRepository, IHttpContextAccessor contextAccessor)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
            _contextAccessor = contextAccessor;
            _userAutneticated = _contextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

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
            var products = (await _productRepository.GetAllWithIncludesAsync(new List<string> { "productType" }));
            var transactions = (await _transactionRepository.GetAllWithIncludesAsync(new List<string> { "transactionType" })).OrderByDescending(t=> t.TransactionDate).ToList();

            List<Product> productsFiltered = products.Where(p => p.UserID == _userAutneticated.Id).ToList();
            List<Transaction> tarnsactionsFiltered = transactions.Where(p => p.UserID == _userAutneticated.Id).ToList();

            DashboardViewModel dvm = new DashboardViewModel
            {
                products = productsFiltered,
                transactions = tarnsactionsFiltered
            };
            return dvm;
        }
    }
}
