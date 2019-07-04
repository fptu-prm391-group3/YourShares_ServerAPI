using System;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using YourShares.Application.Exceptions;
using YourShares.Application.Interfaces;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;

namespace YourShares.Application.Services
{
    public class FacebookAccountService : IFacebookAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<FacebookAccount> _facebookAccountRepository;

        public FacebookAccountService(IRepository<FacebookAccount> facebookAccountRepository, IUnitOfWork unitOfWork)
        {
            _facebookAccountRepository = facebookAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<FacebookAccount> GetById(Guid id)
        {
            return _facebookAccountRepository.GetById(id);
        }

        public async Task<FacebookAccount> GetByFacebookId(string id)
        {
            return _facebookAccountRepository.GetManyAsNoTracking(x => x.FacebookAccountId.Equals(id)).FirstOrDefault();
        }

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
    }
}