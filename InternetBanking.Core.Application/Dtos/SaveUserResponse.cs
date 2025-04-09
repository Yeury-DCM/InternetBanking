using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Dtos
{
    public class SaveUserResponse
    {
        public string UserId { get; set; }
        public bool IsSucess { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
