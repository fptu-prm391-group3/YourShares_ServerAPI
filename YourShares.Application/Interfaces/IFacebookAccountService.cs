using System;
using System.Threading.Tasks;
using YourShares.Domain.Models;

namespace YourShares.Application.Interfaces
{
    public interface IFacebookAccountService
    {
        Task<FacebookAccount> GetById(Guid id);
        Task<FacebookAccount> GetByFacebookId(string id);
        Task<bool> CreateFacebookAccount(Guid userProfileId, string facebookAccountId);
    }
}