using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Contructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="transactionRepository">The transaction repository.</param>
        /// <param name="shareholderRepository">The shareholder repository.</param>
        /// <param name="transactionRequestRepository">The transaction request repository.</param>
        /// <param name="shareAccountRepository">The share account repository.</param>
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
        #endregion

        #region Get By Id        
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Transaction> GetById(Guid id)
        {
            return _transactionRepository.GetById(id);
        }
        #endregion

        #region Get By SharesAccountId        
        /// <summary>
        /// Gets the by shares account identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<List<Transaction>> GetBySharesAccountId(Guid id)
        {
            return _transactionRepository.GetManyAsNoTracking(x => x.ShareAccountId == id).ToList();
        }
        #endregion

        #region Handeling Transaction        
        /// <summary>
        /// Handelings the transaction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="Exception">transaction Request not found</exception>
        public async Task<bool> HandelingTransaction(Guid id, TransactionHandelingModel model)
        {
            var query = _transactionRequestRepository.GetById(id);
            if (query == null) throw new Exception("transaction Request not found");
            if (model.Select == 0)
            {
                var transactionIn = _transactionRepository.GetById(query.TransactionInId);
                transactionIn.TransactionStatusCode = "RJ";
                var transactionOut = _transactionRepository.GetById(query.TransactionOutId);
                transactionOut.TransactionStatusCode = "RJ";

                _transactionRepository.Update(transactionIn);
                _transactionRepository.Update(transactionOut);

            }
            else if (model.Select == 1)
            {
                var transactionIn = _transactionRepository.GetById(query.TransactionInId);
                transactionIn.TransactionStatusCode = "CMP";

                var sharesAccountIn = _shareAccountRepository.GetById(transactionIn.ShareAccountId);
                sharesAccountIn.ShareAmount += transactionIn.TransactionAmount;

                var transactionOut = _transactionRepository.GetById(query.TransactionOutId);
                transactionOut.TransactionStatusCode = "CMP";

                var sharesAccountOut = _shareAccountRepository.GetById(transactionOut.ShareAccountId);
                sharesAccountOut.ShareAmount -= transactionOut.TransactionAmount;

                _transactionRepository.Update(transactionIn);
                _transactionRepository.Update(transactionOut);
                _shareAccountRepository.Update(sharesAccountIn);
                _shareAccountRepository.Update(sharesAccountOut);
            }
            await _unitOfWork.CommitAsync();
            return true;
        }
        #endregion

        #region Request Transaction        
        /// <summary>
        /// Requests the transaction.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Not Enough
        /// or
        /// Receiver not sharehoder Yet
        /// </exception>
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
                Message = model.Message,
                TransactionDate = DateTimeOffset.Now.ToUnixTimeSeconds()
            });

            var transactionOut = _transactionRepository.Insert(new Transaction
            {
                TransactionAmount = model.Amount,
                ShareAccountId = model.ShareAccountId,
                TransactionTypeCode = "OUT",
                TransactionValue = model.Value,
                TransactionStatusCode = "RQ",
                Message = model.Message,
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
        #endregion
    }
}
