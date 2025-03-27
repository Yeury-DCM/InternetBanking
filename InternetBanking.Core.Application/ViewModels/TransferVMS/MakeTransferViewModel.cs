

using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.TransferVMS
{
    public class MakeTransferViewModel
    {
        [Required(ErrorMessage = "Ingrese el nombre de usuario")]
        public string FromAccountNumber { get; set; }

        [Required(ErrorMessage = "Ingrese la cuenta de destino")]
        public string ToAccountNumber { get; set; }

        [Range(0.01, (double) decimal.MaxValue, ErrorMessage ="Ingrese una cantidad válida")]
        public decimal Ammount {  get; set; }
        public bool IsSucess { get; set; } = true;
        public string? ErrorMessage { get; set; }
        
    }
}
