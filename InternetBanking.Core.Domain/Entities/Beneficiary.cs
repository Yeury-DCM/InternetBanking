﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class Beneficiary
    {
        public required string Id { get; set; }
        public required string UserID { get; set; }
        public required int ProductID { get; set; }

        //Navegation properties
  
        public Product account { get; set; }

    }

}
