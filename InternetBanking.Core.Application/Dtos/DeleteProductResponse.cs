

namespace InternetBanking.Core.Application.Dtos
{
    public class DeleteProductResponse
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public bool IsSucess { get; set; }
        public string ErrorMessage { get; set; }    
    }
}
