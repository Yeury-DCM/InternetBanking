using InternetBanking.Core.Application.ViewModels.TransactionVMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface ITransactionService: IGenericService<SaveTransactionViewModel, TransactionViewModel, Transaction>
    {
    }
}
