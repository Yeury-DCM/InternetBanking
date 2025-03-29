
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Mapping;
using InternetBanking.Core.Application.Services;
using InternetBanking.Core.Application.Services.Factory;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InternetBanking.Core.Application
{
    public static  class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            #region DI
            services.AddScoped<ICreditCardService, CreditCardPaymentService>();
            services.AddScoped<IExpressService, ExpressPaymentService>();
            services.AddScoped<ILoanService, LoanPaymentService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IBeneficiaryService, BeneficiaryService>();
            services.AddScoped<PaymentServiceFactory>();


            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(m => m.AddProfile(new GeneralProfile()), typeof(GeneralProfile));
            #endregion
        }
    }
}
