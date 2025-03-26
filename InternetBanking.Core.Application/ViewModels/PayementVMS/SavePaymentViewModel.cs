using InternetBanking.Core.Application.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Application.ViewModels.ProductVMS;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.PayementVMS
{
    public class SavePaymentViewModel
    {
        public  int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar una cuenta origen")]
        [DataType(DataType.Text)]
        public  string OriginProduct { get; set; }

        [Required(ErrorMessage = "Debe ingresar una cuenta destino")]
        [DataType(DataType.Text)]
        public  string DestinationProduct { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        [DataType(DataType.Currency)]
        public  decimal Amount { get; set; }
        public  decimal? Limit { get; set; }

        public PaymentType PaymentType { get; set; }

        public ICollection<ProductViewModel> products { get; set; } = new List<ProductViewModel>();
        //public ICollection<Beneficiary> beneficiaries { get; set; } = new List<Beneficiary>();
    }
}
