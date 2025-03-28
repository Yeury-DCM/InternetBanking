using InternetBanking.Core.Application.ViewModels.Base;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.ProductVMS
{
    public class SaveProductViewModel : IHasId
    {
        public  int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un numero de 9 digitos al producto")]
        [DataType(DataType.Text)]
        public string ProductNumber { get; set; }

        [Required(ErrorMessage = "Debe expecificar el tipo de producto")]
        public int ProductTypeID { get; set; }

        [Required(ErrorMessage ="Debe ingresar un balance inicial")]
        public  decimal Balance { get; set; }

        [Required(ErrorMessage = "Debe ingresar un limite")]
        public decimal? Limit { get; set; }

        public bool IsPrincipal { get; set; } = false;

        [Required(ErrorMessage = "Debe asignarle el producto a un cliente")]
        public required string UserID { get; set; }

        
    }
}
