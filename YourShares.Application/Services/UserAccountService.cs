using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using YourShares.Application.Exceptions;
using YourShares.Application.Interfaces;
using YourShares.Application.ViewModels;
using YourShares.Data.Interfaces;
using YourShares.Data.UoW;
using YourShares.Domain.Models;
using YourShares.Domain.Util;

namespace YourShares.Application.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserAccount> _userAccountRepository;

        public UserAccountService(IRepository<UserAccount> userAccountRepository, IUnitOfWork unitOfWork)
        {
            _userAccountRepository = userAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserAccount> GetById(Guid id)
        {
            var result = _userAccountRepository.GetById(id);
            if (result == null) throw new EntityNotFoundException($"User account id {id} not found");
            return result;
        }

        public async Task<bool> CreateUserAccount(UserAccountCreateModel model, Guid userProfileId)
        {
            if (!ValidateUtils.IsMail(model.Email)) throw new MalformedEmailException();
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
    }
}