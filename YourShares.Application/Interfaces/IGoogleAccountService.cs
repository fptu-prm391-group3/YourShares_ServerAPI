using System;
using System.Threading.Tasks;
using YourShares.Domain.Models;

namespace YourShares.Application.Interfaces
{
    public interface IUserGoogleAccountService
    {
        Task<GoogleAccount> GetById(Guid id);

        Task<GoogleAccount> GetByGoogleId(string id);

        Task<bool> CreateGoogleAccount(Guid userProfileId, string googleAccountId);
    }
}