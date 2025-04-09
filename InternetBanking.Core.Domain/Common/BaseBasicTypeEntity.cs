using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Common
{
    public class BaseBasicTypeEntity
    {
        public virtual int Id { get; set; }
        public required string Type { get; set; }
    }
}
