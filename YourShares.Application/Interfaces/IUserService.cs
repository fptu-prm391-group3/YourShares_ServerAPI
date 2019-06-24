using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YourShares.Application.ViewModels;

namespace YourShares.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> UpdateInfo(UserEditInfoModel model);

        Task<bool> UpdateEmail(UserEditEmailModel model);
    }
}
