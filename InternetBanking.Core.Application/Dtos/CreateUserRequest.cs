
namespace InternetBanking.Core.Application.Dtos
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ImagePath { get; set; }
        public string PhoneNumber { get; set; }
    }
}
