using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.AdvanceCashVMS
{
    public class AdvanceCashViewModel
    {
        public int CreditCardId { get; set; }
        public int SavingsAccountId { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Amount { get; set; }

        public decimal InterestRate { get; set; } = 6.25M; // Tasa de interés fija
    }

}
