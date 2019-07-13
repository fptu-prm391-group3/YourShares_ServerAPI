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

        #region Contructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="SharesAccountService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="shareAccountRepository">The share account repository.</param>
        /// <param name="shareholderRepository">The shareholder repository.</param>
        /// <param name="restrictedShareRepository">The restricted share repository.</param>
        /// <param name="companyRepository">The company repository.</param>
        /// <param name="userProfileRepository">The user profile repository.</param>
        /// <param name="restrictedShareService">The restricted share service.</param>
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
        #endregion

        #region Add Restricted Shares        
        /// <summary>
        /// Adds the restricted shares.
        /// </summary>
        /// <param name="shareholderId">The shareholder identifier.</param>
        /// <param name="restricted">The restricted.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
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
        #endregion

        #region Create        
        /// <summary>
        /// Creates the shares account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<ShareAccount> CreateSharesAccount(SharesAccountCreateModel model)
        {
            var sharesAccount = new ShareAccount
            {
                ShareAmount = model.ShareAmount,
                ShareholderId = model.ShareholderId,
                ShareTypeCode = model.ShareTypeCode
            };
            var result= _shareAccountRepository.Insert(sharesAccount).Entity;
            await _unitOfWork.CommitAsync();
            return result;
        }
        #endregion

        #region Delete        
        /// <summary>
        /// Deletes the share account.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">SharesAccount not found</exception>
        public async Task DeleteShareAccount(Guid id)
        {
            var query = _shareAccountRepository.GetById(id);
            if (query == null) throw new Exception("SharesAccount not found");
            _shareAccountRepository.Delete(query);
            await _unitOfWork.CommitAsync();
        }
        #endregion

        #region Get By Id        
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<SharesAccountDetailModel> GetById(Guid id)
        {
            var query = _shareAccountRepository.GetById(id);
            return new SharesAccountDetailModel
            {
                Name = query.ShareTypeCode.Contains(RefShareTypeCode.Restricted) ? "Restricted" : "Standard",
                ShareAmount = query.ShareAmount,
                ShareAccountId = query.ShareAccountId,
                ShareholderId = query.ShareholderId
            };
        }
        #endregion

        #region Get By Shareholder Id        
        /// <summary>
        /// Gets the by shareholder identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<List<SharesAccountDetailModel>> GetByShareholderId(Guid id)
        {
            var result = _shareAccountRepository.GetManyAsNoTracking(x => x.ShareholderId == id)
                .Select(x=> new SharesAccountDetailModel {
                    Name = x.ShareTypeCode.Contains(RefShareTypeCode.Restricted) ? "Restricted" : "Standard",
                    ShareAmount = x.ShareAmount,
                    ShareAccountId = x.ShareAccountId,
                    ShareholderId = x.ShareholderId
                }).ToList();
            return result;
        }
        #endregion

        #region Update        
        /// <summary>
        /// Updates the share account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="Exception">SharesAccount not found</exception>
        public async Task<ShareAccount> UpdateShareAccount(SharesAccountEditModel model)
        {
            var query = _shareAccountRepository.GetById(model.Id);
            if (query == null) throw new Exception("SharesAccount not found");
            query.ShareAmount = model.ShareAmount;
            query.ShareTypeCode = model.ShareTypeCode;
            _shareAccountRepository.Update(query);
            await _unitOfWork.CommitAsync();
            return query;
        }
        #endregion

        #region View all        
        /// <summary>
        /// Views all shares account of company.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        public async Task<List<ShareAccountViewAllModel>> ViewAllSharesAccountOfCompany(Guid companyId)
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
                    ListAccount = await ViewSharesAccountOfUserInCompany(new ShareAccountGetDetailModel
                    {
                        CompanyId = companyId,
                        UserId = x.UserProfileId,
                    }),
                    UserName = x.Name
                });
            var result = await Task.WhenAll(query);
            return result.ToList();
        }
        #endregion

        #region View        
        /// <summary>
        /// Views the shares account of user in company.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<List<SharesAccountViewModel>> ViewSharesAccountOfUserInCompany(ShareAccountGetDetailModel model)
        {
            var totalShares = (float)_companyRepository.GetById(model.CompanyId).TotalShares;

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
                            ? 0 : ((float)x.shareAccount.ShareAmount / totalShares * 100),
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
        #endregion
    }
}
