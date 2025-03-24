﻿using InternetBanking.Core.Application.ViewModels.ProductVMS;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IProductService: IGenericService<SaveProductViewModel, ProductViewModel, Product>
    {
    }
}
