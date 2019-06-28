using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
using YourShares.Data.Interfaces;

namespace YourShares.Application.Interfaces
{
    public interface IShareholderService
    {
        Task<bool> AddUserAsShareHolder(ShareHolderAddUserModel model,string currentUser);

        Task<List<ShareholderSearchViewModel>> SearchShareholder(ShareholderSearchModel model);

        Task<ShareholderSearchViewModel> GetById(Guid id);
    }
}
