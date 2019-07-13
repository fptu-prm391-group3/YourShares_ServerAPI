using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourShares.Application.ViewModels;
using YourShares.Domain.Models;

namespace YourShares.Application.Interfaces
{
    public interface ISharesAccountService
    {
        Task AddRestrictedShares(Guid shareholderId, long restricted, CompanyAddOptionPoolToShareholderModel model);

        Task<List<SharesAccountViewModel>> ViewSharesAccountOfUserInCompany(ShareAccountGetDetailModel model);

        Task<List<ShareAccountViewAllModel>> ViewAllSharesAccountOfCompany(Guid companyId);

        Task<SharesAccountDetailModel> GetById(Guid id);

        Task<List<SharesAccountDetailModel>> GetByShareholderId(Guid id);

        Task DeleteShareAccount(Guid id);

        Task<ShareAccount> UpdateShareAccount(SharesAccountEditModel model);

        Task<ShareAccount> CreateSharesAccount(SharesAccountCreateModel model);
    }
}
