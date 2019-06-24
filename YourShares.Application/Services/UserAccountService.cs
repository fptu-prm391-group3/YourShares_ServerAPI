using System;
using System.Threading.Tasks;
using YourShares.Application.Exceptions;
using YourShares.Application.Interfaces;
using YourShares.Application.ViewModels;
using YourShares.Data.Interfaces;
using YourShares.Data.UoW;
using YourShares.RestApi.Models;

namespace YourShares.Application.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IRepository<UserAccount> _userAccountRepository;

        public UserAccountService(IRepository<UserAccount> userAccountRepository, UnitOfWork unitOfWork)
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

        public Task<UserAccount> CreateUserAccount(UserCreateModel model)
        {
            throw new NotImplementedException();
        }
    }
}