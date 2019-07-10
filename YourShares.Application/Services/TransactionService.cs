using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourShares.Application.Interfaces;
using YourShares.Application.ViewModels;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;

namespace YourShares.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IRepository<Shareholder> _shareholderRepository;
        private readonly IRepository<TransactionRequest> _transactionRequestRepository;
        private readonly IRepository<ShareAccount> _shareAccountRepository;

        public TransactionService(IUnitOfWork unitOfWork
                                , IRepository<Transaction> transactionRepository
                                , IRepository<Shareholder> shareholderRepository
                                , IRepository<TransactionRequest> transactionRequestRepository
                                , IRepository<ShareAccount> shareAccountRepository)
        {
            _unitOfWork = unitOfWork;
            _transactionRepository = transactionRepository;
            _shareAccountRepository = shareAccountRepository;
            _shareholderRepository = shareholderRepository;
            _transactionRequestRepository = transactionRequestRepository;
        }
        public async Task<Transaction> GetById(Guid id)
        {
            return _transactionRepository.GetById(id);
        }

        public async Task<List<Transaction>> GetBySharesAccountId(Guid id)
        {
            return _transactionRepository.GetManyAsNoTracking(x => x.ShareAccountId == id).ToList();
        }

        public async Task<bool> RequestTransaction(TransactionRequestModel model)
        {
            var check = _shareAccountRepository.GetById(model.ShareAccountId)?.ShareAmount;
            if (check < model.Amount)
            {
                throw new Exception("Not Enough");
            }

            // kiem tra thang kia la sharehoder chua
            var check2 = _shareholderRepository.GetManyAsNoTracking(x =>
                             x.CompanyId == model.CompanyId && x.UserProfileId == model.ReceiverProfileId).First();
            if (check2 == null)
            {
                throw new Exception("Receiver not sharehoder Yet");
            }

            var reciverShareAccount = _shareAccountRepository.GetManyAsNoTracking(x =>
                    x.ShareholderId == check2.ShareholderId && x.ShareTypeCode == "STD02").First();
            // tao 2 transaction
            var transactionIn = _transactionRepository.Insert(new Transaction
            {
                TransactionAmount = model.Amount,
                ShareAccountId = reciverShareAccount.ShareAccountId,
                TransactionTypeCode = "IN",
                TransactionValue = model.Value,
                TransactionStatusCode = "RQ",
                TransactionDate = DateTimeOffset.Now.ToUnixTimeSeconds()
            });

            var transactionOut = _transactionRepository.Insert(new Transaction
            {
                TransactionAmount = model.Amount,
                ShareAccountId = model.ShareAccountId,
                TransactionTypeCode = "OUT",
                TransactionValue = model.Value,
                TransactionStatusCode = "RQ",
                TransactionDate = DateTimeOffset.Now.ToUnixTimeSeconds()
            });

            // tao transaction request
            _transactionRequestRepository.Insert(new TransactionRequest
            {
                RequestMessage = model.Message,
                TransactionInId = transactionIn.Entity.TransactionId,
                TransactionOutId = transactionOut.Entity.TransactionId,
            });
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
