using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Infrastructure.Persistence.Contexts;
using InternetBanking.Infrastructure.Persistence.Repositories;
using InternetBanking.Infrastructure.Persistence.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            #region Database configuration
            if (configuration.GetValue<bool>("UseInMemoryDataBase"))
            {
                services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("AppData"));
            }
            else
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString, m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            }
            #endregion


            #region DI
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();
            #endregion
        }


        #region seed
        public async static Task RunProductTypeSeedAsync(this IServiceProvider appServices, IConfiguration configuration)
            {
                using (var scope = appServices.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;

                    try
                    {

                        var applicationContext = serviceProvider.GetRequiredService<ApplicationContext>();


                        await DefaultProductTypes.SeedAsync(applicationContext);

                    }
                    catch (Exception)
                    {
                    }
                }
            }

        #endregion
    }
}

