using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;

namespace YourShares.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> UpdateInfo(UserEditInfoModel model);

        Task<bool> UpdateEmail(UserEditEmailModel model);

        Task<UserViewDetailModel> GetById(Guid id);

        Task<IQueryable<UserSearchViewModel>> SearchUser(UserSearchModel model);
    }
}
