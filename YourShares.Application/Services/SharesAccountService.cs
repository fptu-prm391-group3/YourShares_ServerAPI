using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourShares.Application.Interfaces;
using YourShares.Application.ViewModels;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;

namespace YourShares.Application.Services
{
    public class SharesAccountService : ISharesAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ShareAccount> _shareAccountRepository;
        private readonly IRetrictedSharesService _retrictedSharesService;

        public SharesAccountService(IUnitOfWork unitOfWork
            , IRepository<ShareAccount> shareAccountRepository
            , IRetrictedSharesService retrictedSharesService)
        {
            _unitOfWork = unitOfWork;
            _shareAccountRepository = shareAccountRepository;
            _retrictedSharesService = retrictedSharesService;
        }

        public async Task AddRestrictedShares(Guid ShareholderId, long Restricted, CompanyAddOptionPoolToShareholderModel model)
        {
            var shareAccountId = Guid.Empty;
            var query = _shareAccountRepository.GetManyAsNoTracking(x =>
                              x.ShareholderId == ShareholderId
                              && x.ShareTypeCode.ToLower().Contains(RefShareTypeCode.Restricted)).FirstOrDefault();
            if (query == null)
            {
                var account = new ShareAccount
                {
                    ShareAmount = Restricted,
                    ShareholderId = ShareholderId,
                    ShareTypeCode = RefShareTypeCode.Restricted
                };
                var inserted =_shareAccountRepository.Insert(account).Entity;
                shareAccountId = inserted.ShareAccountId;
            }
            else
            {
                query.ShareAmount = query.ShareAmount + Restricted;
                _shareAccountRepository.Update(query);
                shareAccountId = query.ShareAccountId;
            }
            await _retrictedSharesService.AddRetrictedShares(model.ConvertibleRatio, model.ConvertibleTime, shareAccountId);
        }
    }
}
