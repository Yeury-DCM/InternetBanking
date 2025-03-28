using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.ViewModels.PayementVMS;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaymentViewModel> ProcessPaymentData(SavePaymentViewModel vm);
        Task ConfirmPayment(PaymentViewModel vm);
        async Task<Product> GetProductWithIncludes(string pNumber, IPaymentRepository repository)
        {
            var product = await repository.GetByNumberWithIncludesAsync(pNumber, new List<string> {"productType" });
            return product;
        }
    }
}
