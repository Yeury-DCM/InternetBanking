using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.TransactionVMS
{
     public class SaveTransactionViewModel
    {
        public  int Id { get; set; }

        [Required(ErrorMessage ="Debe ingresar un usuario")]
        public required int UserID { get; set; }

        [Required(ErrorMessage = "Debe ingresar un producto")]
        public required int ProductID { get; set; }

        [Required(ErrorMessage = "Debe ingresar un Tipo de transaccion")]
        public required int TransactionTypeID { get; set; }

        [Required(ErrorMessage = "Debe ingresar el monto de la transaccion")]
        public required decimal Amount { get; set; }

        [Required(ErrorMessage = "Debe ingresar un la fecha de la transaccion")]
        public required DateTime TransactionDate { get; set; }

       
    }
}
