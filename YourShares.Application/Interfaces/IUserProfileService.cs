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
        Task<bool> UpdateInfo(UserEditInfoModel model);

        Task<bool> UpdateEmail(Guid id, string email);

        Task<UserViewDetailModel> GetById(Guid id);

        Task<List<UserSearchViewModel>> SearchUser(UserSearchModel model);

        Task<UserAccount> GetUserByEmail(string email);

        Task<bool> CreateUserProfile(UserRegisterModel profileModel, UserAccountCreateModel accountCreateModel);

        Task<bool> CreateGoogleProfile(UserRegisterModel profileModel, string googleAccountId);

        Task<bool> CreateFacebookAccountProfile(UserRegisterModel profileModel, string facebookAccountId);
    }
}
