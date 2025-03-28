

using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;

namespace InternetBanking.Infrastructure.Persistence.Seeds
{
    public static class DefaultProductTypes
    {
        public async static Task SeedAsync(ApplicationContext context)
        {
            ProductType savingAccount = new()
            {
                Id = 1,
                Type = "SavingAccount"
            };

            ProductType creditCard = new()
            {
               Id =2,
                Type = "CreditCard"
            };

            ProductType loan = new()
            {
                Id = 3,
                Type = "Loan"
            };


            if(await context.Set<ProductType>().FindAsync(1) ==  null)
            {
                context.Set<ProductType>().Add(savingAccount);
            }

            if (await context.Set<ProductType>().FindAsync(2) == null)
            {
                context.Set<ProductType>().Add(creditCard);
            }

            if (await context.Set<ProductType>().FindAsync(3) == null)
            {
                context.Set<ProductType>().Add(loan);
            }

            context.SaveChanges();
        }
    }
}
