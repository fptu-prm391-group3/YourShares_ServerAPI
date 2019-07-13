using System;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using YourShares.Application.Interfaces;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;

namespace YourShares.Application.Services
{
    public class FacebookAccountService : IFacebookAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<FacebookAccount> _facebookAccountRepository;

        #region Contructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAccountService"/> class.
        /// </summary>
        /// <param name="facebookAccountRepository">The facebook account repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public FacebookAccountService(IRepository<FacebookAccount> facebookAccountRepository, IUnitOfWork unitOfWork)
        {
            _facebookAccountRepository = facebookAccountRepository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Get By Id        
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<FacebookAccount> GetById(Guid id)
        {
            return _facebookAccountRepository.GetById(id);
        }
        #endregion

        #region Gets the by facebook identifier
        /// <summary>
        /// Gets the by facebook identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<FacebookAccount> GetByFacebookId(string id)
        {
            return _facebookAccountRepository.GetManyAsNoTracking(x => x.FacebookAccountId.Equals(id)).FirstOrDefault();
        }
        #endregion

        #region Create Facebook Account        
        /// <summary>
        /// Creates the facebook account.
        /// </summary>
        /// <param name="userProfileId">The user profile identifier.</param>
        /// <param name="facebookAccountId">The facebook account identifier.</param>
        /// <returns></returns>
        public async Task<bool> CreateFacebookAccount(Guid userProfileId, string facebookAccountId)
        {
            var facebookAccount = new FacebookAccount
            {
                UserProfileId = userProfileId,
                FacebookAccountId = facebookAccountId
            };
            _facebookAccountRepository.Insert(facebookAccount);
            await _unitOfWork.CommitAsync();
            return true;
        }
        #endregion
    }
}