using System;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using YourShares.Application.Interfaces;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;

namespace YourShares.Application.Services
{
    public class GoogleAccountService : IUserGoogleAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<GoogleAccount> _googleAccountRepository;

        #region Contructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAccountService"/> class.
        /// </summary>
        /// <param name="googleAccountRepository">The google account repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public GoogleAccountService(IRepository<GoogleAccount> googleAccountRepository, IUnitOfWork unitOfWork)
        {
            _googleAccountRepository = googleAccountRepository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region GetById        
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<GoogleAccount> GetById(Guid id)
        {
            return _googleAccountRepository.GetById(id);
        }
        #endregion

        #region Get By Google Id        
        /// <summary>
        /// Gets the by google identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<GoogleAccount> GetByGoogleId(string id)
        {
            return _googleAccountRepository.GetManyAsNoTracking(x => x.GoogleAccountId.Equals(id)).FirstOrDefault();
        }
        #endregion

        #region Create Google Account        
        /// <summary>
        /// Creates the google account.
        /// </summary>
        /// <param name="userProfileId">The user profile identifier.</param>
        /// <param name="googleAccountId">The google account identifier.</param>
        /// <returns></returns>
        public async Task<bool> CreateGoogleAccount(Guid userProfileId, string googleAccountId)
        {
            var googleAccount = new GoogleAccount
            {
                UserProfileId = userProfileId,
                GoogleAccountId = googleAccountId
            };
            _googleAccountRepository.Insert(googleAccount);
            await _unitOfWork.CommitAsync();
            return true;
        }
        #endregion
    }
}