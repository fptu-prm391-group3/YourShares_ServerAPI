using System;
using System.Threading.Tasks;
using YourShares.Application.ViewModels;
using YourShares.RestApi.Models;

namespace YourShares.Application.Interfaces
{
    public interface IUserAccountService
    {
        Task<UserAccount> GetById(Guid id);

        Task<UserAccount> CreateUserAccount(UserCreateModel model);
    }
}