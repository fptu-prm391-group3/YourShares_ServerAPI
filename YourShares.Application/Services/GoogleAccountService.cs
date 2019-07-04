using System;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using YourShares.Application.Exceptions;
using YourShares.Application.Interfaces;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;

namespace YourShares.Application.Services
{
    public class GoogleAccountService : IUserGoogleAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<GoogleAccount> _googleAccountRepository;

        public GoogleAccountService(IRepository<GoogleAccount> googleAccountRepository, IUnitOfWork unitOfWork)
        {
            _googleAccountRepository = googleAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<GoogleAccount> GetById(Guid id)
        {
            return _googleAccountRepository.GetById(id);
        }

        public async Task<GoogleAccount> GetByGoogleId(string id)
        {
            return _googleAccountRepository.GetManyAsNoTracking(x => x.GoogleAccountId.Equals(id)).FirstOrDefault();
        }

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
    }
}