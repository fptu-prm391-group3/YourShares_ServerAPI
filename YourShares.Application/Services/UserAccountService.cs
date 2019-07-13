using System;
using System.Threading.Tasks;
using YourShares.Application.Exceptions;
using YourShares.Application.Interfaces;
using YourShares.Application.ViewModels;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;
using YourShares.Domain.Util;

namespace YourShares.Application.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserAccount> _userAccountRepository;

        #region Contructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountService"/> class.
        /// </summary>
        /// <param name="userAccountRepository">The user account repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public UserAccountService(IRepository<UserAccount> userAccountRepository, IUnitOfWork unitOfWork)
        {
            _userAccountRepository = userAccountRepository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Get By Id        
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">User account id {id} not found</exception>
        public async Task<UserAccount> GetById(Guid id)
        {
            var result = _userAccountRepository.GetById(id);
            if (result == null) throw new EntityNotFoundException($"User account id {id} not found");
            return result;
        }
        #endregion

        #region Create User Account        
        /// <summary>
        /// Creates the user account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="userProfileId">The user profile identifier.</param>
        /// <returns></returns>
        /// <exception cref="FormatException">
        /// Email invalid
        /// or
        /// Password invalid
        /// </exception>
        public async Task<bool> CreateUserAccount(UserAccountCreateModel model, Guid userProfileId)
        {
            if (!ValidateUtils.IsMail(model.Email)) throw new FormatException("Email invalid");
            if (model.Password.Length < 8) throw new FormatException("Password invalid");
            var passwordSalt = Guid.NewGuid();
            var data = HashingUtils.GetHashData(model.Password + passwordSalt);
            _userAccountRepository.Insert(new UserAccount
            {
                Email = model.Email,
                PasswordHash = data.DataHashed,
                PasswordHashAlgorithm = data.HashType,
                UserProfileId = userProfileId,
                PasswordSalt = passwordSalt,
                UserAccountStatusCode = RefUserAccountStatusCode.Guest
            });
            await _unitOfWork.CommitAsync();
            return true;
        }
        #endregion
    }
}