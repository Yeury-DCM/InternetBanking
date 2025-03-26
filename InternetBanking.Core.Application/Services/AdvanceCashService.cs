using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.ViewModels.AdvanceCashVMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class AdvanceCashService
    {
        private readonly IProductRepository _productRepository;

        public AdvanceCashService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task AdvanceCash(AdvanceCashViewModel vm)
        {
            var creditCard = await _productRepository.GetByIdAsync(vm.CreditCardId);
            var savingsAccount = await _productRepository.GetByIdAsync(vm.SavingsAccountId);

            if (creditCard == null || savingsAccount == null)
                throw new Exception("Tarjeta de crédito o cuenta de ahorro no encontrada.");

            if (creditCard.productType.Type != "Tarjeta de Credito" || savingsAccount.productType.Type != "Cuenta de ahorro")
                throw new Exception("Productos no válidos para esta operación.");

            if (creditCard.Limit.HasValue && (creditCard.Limit.Value - creditCard.Balance) < vm.Amount) 
                throw new Exception("El monto solicitado supera el límite de crédito disponible.");

            decimal interest = vm.Amount * (vm.InterestRate / 100);
            decimal totalDebt = vm.Amount + interest;

            creditCard.Balance += totalDebt;
            savingsAccount.Balance += vm.Amount;

            await _productRepository.UpdateAsync(creditCard, creditCard.Id);
            await _productRepository.UpdateAsync(savingsAccount, savingsAccount.Id);
        }
    }
}