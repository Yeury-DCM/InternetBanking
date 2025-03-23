using InternetBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class UserType: BaseBasicTypeEntity
    {
        public ICollection<User> users { get; set; } = new List<User>();
    }
}
