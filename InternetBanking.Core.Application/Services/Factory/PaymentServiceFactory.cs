using InternetBanking.Core.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services.Factory
{
    public class PaymentServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentService GetService(PaymentType paymentType)
        {
            return paymentType switch
            {
                PaymentType.Express => _serviceProvider.GetRequiredService<IExpressService>(),
                PaymentType.CreditCard => _serviceProvider.GetRequiredService<ICreditCardService>(),
                PaymentType.Loan => _serviceProvider.GetRequiredService<ILoanService>(),
                PaymentType.Transfer=>_serviceProvider.GetRequiredService<ITransferService>(),
                _ => throw new InvalidOperationException("Tipo de pago no válido")
            };
        }
    }
}
