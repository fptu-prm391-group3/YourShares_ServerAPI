using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YourShares.Application.ViewModels;

namespace YourShares.Application.Interfaces
{
    public interface ISharesAccountService
    {
        Task AddRestrictedShares(Guid shareholderId, long restricted, CompanyAddOptionPoolToShareholderModel model);

        Task<List<SharesAccountViewModel>> ViewSharesAccountOfUserInCompany(ShareAccountGetDetailModel model);

        Task<List<ShareAccountViewAllModel>> ViewAllSharesAccountOfCompany(Guid companyId);

        Task<SharesAccountDetailModel> GetById(Guid id);

        Task<List<SharesAccountDetailModel>> GetByShareholderId(Guid id);
    }
}
