using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YourShares.Application.ViewModels;

namespace YourShares.Application.Interfaces
{
    public interface ISharesAccountService
    {
        Task AddRestrictedShares(Guid ShareholderId, long Restricted, CompanyAddOptionPoolToShareholderModel model);

        Task<List<SharesAccountViewModel>> ViewSharesAccount(ShareAccountGetDetailModel model);

        Task<List<ShareAccountViewAllModel>> ViewAllSharesAccount(Guid companyId);
    }
}
