
﻿using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;
using System.Threading.Tasks;
using YourShares.Application.ViewModels;
 using YourShares.RestApi.Models;

 namespace YourShares.Application.Interfaces
{
    public interface IUserProfileService
    {
        Task<bool> UpdateInfo(UserEditInfoModel model);

        Task<bool> UpdateEmail(UserEditEmailModel model);

        Task<UserViewDetailModel> GetById(Guid id);

        Task<List<UserViewModel>> SearchUserByEmail(string email, int maxResult);

        Task<UserProfile> CreateUserProfile(UserCreateModel model);
    }
}
