

using InternetBanking.Core.Application.Enums;

namespace InternetBanking.Core.Application.ViewModels.UserVMS
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string IdentificationNumer { get; set; }
        public List<string> Roles { get; set; }
        public bool IsActive { get; set; }

    }
}
