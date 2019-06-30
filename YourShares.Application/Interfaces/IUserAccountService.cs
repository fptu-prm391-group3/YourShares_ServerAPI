using System;
using System.Threading.Tasks;
using YourShares.Application.ViewModels;
using YourShares.Domain.Models;

namespace YourShares.Application.Interfaces
{
    public interface IUserAccountService
    {
        Task<UserAccount> GetById(Guid id);

        Task<bool> CreateUserAccount(UserAccountCreateModel model, Guid userProfileId);
    }
}