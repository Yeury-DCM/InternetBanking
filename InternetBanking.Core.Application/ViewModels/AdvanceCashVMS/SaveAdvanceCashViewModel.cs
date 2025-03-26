using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.AdvanceCashVMS
{
    public class SaveAdvanceCashViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar una tarjeta de crédito.")]
        public int CreditCardId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una cuenta de ahorro.")]
        public int SavingsAccountId { get; set; }

        [Required(ErrorMessage = "El monto del avance es requerido.")]
        [Range(1, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Amount { get; set; }

        [Display(Name = "Tasa de Interés (%)")]
        public decimal InterestRate { get; set; } = 6.25M; // Tasa de interés predeterminada.
    }
}
