using System;
 using System.Collections.Generic;
using System.Threading.Tasks;
 using YourShares.Application.SearchModels;
 using YourShares.Application.ViewModels;

namespace YourShares.Application.Interfaces
{
    public interface IUserProfileService
    {
        Task<bool> UpdateInfo(UserEditInfoModel model);

        Task<bool> UpdateEmail(UserEditEmailModel model);

        Task<UserViewDetailModel> GetById(Guid id);

        Task<List<UserSearchViewModel>> SearchUser(UserSearchModel model);

        Task<UserLoginViewModel> GetUserByEmail(string email);

        Task<bool> CreateUserProfile(UserRegisterModel profileModel, UserAccountCreateModel accountCreateModel);
    }
}
