

using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.AccountVMS
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ingrese el nombre de usuario")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ingrese la contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsSucess { get; set; } = true;
        public string? ErrorMessage { get; set; }
    }
}
