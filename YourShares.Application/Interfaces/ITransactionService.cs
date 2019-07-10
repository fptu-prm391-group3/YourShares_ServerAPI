using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YourShares.Application.ViewModels;
using YourShares.Domain.Models;

namespace YourShares.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> GetById(Guid id);
        Task<List<Transaction>> GetBySharesAccountId(Guid id);
        Task<bool> RequestTransaction(TransactionRequestModel model);
    }
}
