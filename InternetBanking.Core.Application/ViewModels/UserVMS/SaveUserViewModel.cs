using InternetBanking.Core.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.UserVMS
{
    public class SaveUserViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Roles UserType { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string IdentificationNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsSucess { get; set; }
        public string? ErrorMessage { get; set; }
        public decimal InitialAmount { get; set; }  
    }
}
