using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Common
{
    public class BaseBasicTypeEntity
    {
        public required int Id { get; set; }
        public required int Type { get; set; }
    }
}
