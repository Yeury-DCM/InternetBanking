﻿using InternetBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
   public  class TransactionType: BaseBasicTypeEntity
    {
        public ICollection<Transaction> transactions {  get; set; }= new List<Transaction>();
    }
}
