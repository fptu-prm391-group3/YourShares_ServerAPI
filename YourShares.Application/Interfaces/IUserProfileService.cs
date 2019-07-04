using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
using YourShares.Domain.Models;

namespace YourShares.Application.Interfaces
{
    public interface IUserProfileService
    {

        Task<UserProfile> UpdateLastName(Guid id, string lastName);

        Task<UserProfile> UpdateFirstName(Guid id, string firstName);

        Task<UserProfile> UpdateAddress(Guid id, string address);

        Task<UserProfile> UpdatePhone(Guid id, string phone);

        Task<UserProfile> UpdateEmail(Guid id, string email);

        Task<bool> UpdateInfo(UserEditInfoModel model);

        Task<UserProfile> GetById(Guid id);

        Task<List<UserSearchViewModel>> SearchUser(UserSearchModel model);

        Task<UserAccount> GetUserByEmail(string email);

        Task<bool> CreateUserProfile(UserRegisterModel profileModel, UserAccountCreateModel accountCreateModel);

        Task<bool> CreateGoogleProfile(OAuthCreateModel profileModel);

        Task<bool> CreateFacebookProfile(OAuthCreateModel profileModel);
    }
}
