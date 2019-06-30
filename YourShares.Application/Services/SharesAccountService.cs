using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IRepository<Shareholder> _shareholderRepository;
        private readonly IRepository<RestrictedShare> _restrictedShareRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IRetrictedSharesService _restrictedShareService;

        public SharesAccountService(IUnitOfWork unitOfWork
            , IRepository<ShareAccount> shareAccountRepository
            , IRepository<Shareholder> shareholderRepository
            , IRepository<RestrictedShare> restrictedShareRepository
            , IRepository<Company> companyRepository
            , IRepository<UserProfile> userProfileRepository
            , IRetrictedSharesService restrictedShareService)
        {
            _unitOfWork = unitOfWork;
            _shareAccountRepository = shareAccountRepository;
            _restrictedShareService = restrictedShareService;
            _shareholderRepository = shareholderRepository;
            _restrictedShareRepository = restrictedShareRepository;
            _companyRepository = companyRepository;
            _companyRepository = companyRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task AddRestrictedShares(Guid shareholderId, long restricted, 
            CompanyAddOptionPoolToShareholderModel model)
        {
            Guid shareAccountId;
            var query = _shareAccountRepository.GetManyAsNoTracking(x =>
                              x.ShareholderId == shareholderId
                              && x.ShareTypeCode.ToLower().Contains(RefShareTypeCode.Restricted)).FirstOrDefault();
            if (query == null)
            {
                var account = new ShareAccount
                {
                    ShareAmount = restricted,
                    ShareholderId = shareholderId,
                    ShareTypeCode = RefShareTypeCode.Restricted
                };
                var inserted = _shareAccountRepository.Insert(account).Entity;
                shareAccountId = inserted.ShareAccountId;
            }
            else
            {
                query.ShareAmount += restricted;
                _shareAccountRepository.Update(query);
                shareAccountId = query.ShareAccountId;
            }
            await _restrictedShareService.AddRetrictedShares(model.ConvertibleRatio, model.ConvertibleTime, shareAccountId);
        }

        public async Task<List<ShareAccountViewAllModel>> ViewAllSharesAccount(Guid companyId)
        {
            var query = _shareholderRepository.GetManyAsNoTracking(x => x.CompanyId == companyId)
                .Join(_userProfileRepository.GetAllAsNoTracking(),
                x => x.UserProfileId, y => y.UserProfileId, (x, y) => new
                {
                    Shareholder = x,
                    Name = $"{y.FirstName} {y.LastName}",
                    y.UserProfileId
                }).ToList()
                .Select(async x => new ShareAccountViewAllModel
                {
                    Type = x.Shareholder.ShareholderTypeCode,
                    ListAccount = await ViewSharesAccount(new ShareAccountGetDetailModel
                    {
                        CompanyId = companyId,
                        UserId = x.UserProfileId,
                    }),
                    UserName = x.Name
                });
            var result = await Task.WhenAll(query);
            return result.ToList();
        }

        public async Task<List<SharesAccountViewModel>> ViewSharesAccount(ShareAccountGetDetailModel model)
        {
            var TotalShares = (float)_companyRepository.GetById(model.CompanyId)?.TotalShares;

            var query = _shareholderRepository.GetManyAsNoTracking(x =>
                         x.UserProfileId == model.UserId && x.CompanyId == model.CompanyId)
                    .Join(_shareAccountRepository.GetAll(), x =>
                    x.ShareholderId, y => y.ShareholderId, (x, y) => new
                    {
                        Shareholder = x,
                        shareAccount = y
                    }).ToList();

            var result = query.Select(x => new SharesAccountViewModel
            {
                ShareAccountId = x.shareAccount.ShareAccountId,
                ShareAmount = x.shareAccount.ShareAmount,
                ShareAmountRatio = x.shareAccount.ShareTypeCode.Contains(RefShareTypeCode.Restricted)
                            ? 0 : ((float)x.shareAccount.ShareAmount / TotalShares * 100),
                Name = x.shareAccount.ShareTypeCode.Contains(RefShareTypeCode.Restricted) ? "Restricted" : "Standard",
                RatioConvert = x.shareAccount.ShareTypeCode.Contains(RefShareTypeCode.Restricted) ?
                            (float)_restrictedShareRepository.GetById(x.shareAccount.ShareAccountId).ConvertibleRatio : 0,
                TimeConvert = x.shareAccount.ShareTypeCode.Contains(RefShareTypeCode.Restricted) ?
                            new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(
                                _restrictedShareRepository.GetById(x.shareAccount.ShareAccountId).AssignDate +
                                _restrictedShareRepository.GetById(x.shareAccount.ShareAccountId).ConvertibleTime) :
                            new DateTime()
            }).ToList();

            return result;
        }
    }
}
