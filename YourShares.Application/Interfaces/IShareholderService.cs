using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
using YourShares.Domain.Models;

namespace YourShares.Application.Interfaces
{
    public interface IShareholderService
    {
        Task<Shareholder> AddUserAsShareHolder(ShareHolderAddUserModel model,string currentUser);

        Task<List<ShareholderSearchViewModel>> SearchShareholder(ShareholderSearchModel model);

        Task<ShareholderSearchViewModel> GetById(Guid id);

        Task<List<ShareholderDetailModel>> GetByCompanyId(Guid id);

        Task<List<ShareholderDetailModel>> GetByUserId(Guid id);

        Task DeleteShareholder(Guid id);

        Task<Shareholder> UpdateSharesHolder(ShareHolderUpdateModel model);

    }
}
