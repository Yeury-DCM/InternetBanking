
using InternetBanking.Core.Application.Enums;

namespace InternetBanking.Core.Application.Dtos
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
        public bool IsSucess { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
