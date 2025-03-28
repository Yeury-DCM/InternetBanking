
using InternetBanking.Core.Application.Enums;

namespace InternetBanking.Core.Application.Dtos
{
    public class SaveUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IdentificationNumber { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserId { get; set; }
        public bool IsSucess { get; set; }
        public Roles Role {  get; set; }
        public string? ErrorMessage { get; set; }
    }
}
