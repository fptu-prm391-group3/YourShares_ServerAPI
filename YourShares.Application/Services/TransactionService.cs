using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourShares.Application.Interfaces;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;

namespace YourShares.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Transaction> _transactionRepository;

        public TransactionService(IUnitOfWork unitOfWork
                                , IRepository<Transaction> transactionRepository)
        {
            _unitOfWork = unitOfWork;
            _transactionRepository = transactionRepository;
        }
        public async Task<Transaction> GetById(Guid id)
        {
            return _transactionRepository.GetById(id);
        }

        public async Task<List<Transaction>> GetBySharesAccountId(Guid id)
        {
            return  _transactionRepository.GetManyAsNoTracking(x => x.ShareAccountId == id).ToList();
        }
    }
}
