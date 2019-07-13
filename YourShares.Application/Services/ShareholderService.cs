using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using YourShares.Application.Exceptions;
using YourShares.Application.Interfaces;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;
using YourShares.Domain.Util;

namespace YourShares.Application.Services
{
    public class ShareholderService : IShareholderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Shareholder> _shareholderRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<UserProfile> _userProfileRepository;

        #region Contructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="ShareholderService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="shareholderRepository">The shareholder repository.</param>
        /// <param name="companyRepository">The company repository.</param>
        /// <param name="userProfileRepository">The user profile repository.</param>
        public ShareholderService(IUnitOfWork unitOfWork, IRepository<Shareholder> shareholderRepository,
                                 IRepository<Company> companyRepository, IRepository<UserProfile> userProfileRepository)
        {
            _unitOfWork = unitOfWork;
            _shareholderRepository = shareholderRepository;
            _companyRepository = companyRepository;
            _userProfileRepository = userProfileRepository;
        }
        #endregion

        #region Add User As ShareHolder        
        /// <summary>
        /// Adds the user as share holder.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="currentUserId">The current user identifier.</param>
        /// <returns></returns>
        /// <exception cref="FormatException">Invalid shareholder type</exception>
        /// <exception cref="EntityNotFoundException">Company {model.CompanyId} not found</exception>
        public async Task<Shareholder> AddUserAsShareHolder(ShareHolderAddUserModel model, string currentUserId)
        {
            var id = Guid.Parse(currentUserId);
            if (!new List<string> {RefShareholderTypeCode.Founders,
                RefShareholderTypeCode.Shareholders,
                RefShareholderTypeCode.Employees}
                .Contains(model.ShareholderType))
            {
                throw new FormatException("Invalid shareholder type");
            }
            var company = _companyRepository.GetManyAsNoTracking(x => x.AdminProfileId == id && x.CompanyId == model.CompanyId);
            if (company == null) throw new EntityNotFoundException($"Company {model.CompanyId} not found");
            var shareholder = new Shareholder
            {
                CompanyId = model.CompanyId,
                UserProfileId = model.UserId,
                ShareholderTypeCode = model.ShareholderType
            };
            var inserted = _shareholderRepository.Insert(shareholder).Entity;
            await _unitOfWork.CommitAsync();
            return inserted;
        }
        #endregion

        #region Delete        
        /// <summary>
        /// Deletes the shareholder.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">shareholder id {id} not found</exception>
        public async Task DeleteShareholder(Guid id)
        {
            var result = _shareholderRepository.GetById(id);
            if (result == null) throw new EntityNotFoundException($"shareholder id {id} not found");
            _shareholderRepository.Delete(result);
            await _unitOfWork.CommitAsync();
        }
        #endregion

        #region Get By CompanyId        
        /// <summary>
        /// Gets the by company identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<List<ShareholderDetailModel>> GetByCompanyId(Guid id)
        {
            var result = _shareholderRepository.GetManyAsNoTracking(x => x.CompanyId == id)
                        .Select(x => new ShareholderDetailModel
                        {
                            CompanyId = x.CompanyId,
                            ShareholderId = x.ShareholderId,
                            ShareholderType = x.ShareholderTypeCode,
                            UserProfileId = x.UserProfileId
                        }).ToList();
            return result;
        }
        #endregion

        #region Get By Id        
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">shareholder id {id} not found</exception>
        public async Task<ShareholderSearchViewModel> GetById(Guid id)
        {
            var result = _shareholderRepository.GetById(id);
            if (result == null) throw new EntityNotFoundException($"shareholder id {id} not found");
            var query = _shareholderRepository.GetManyAsNoTracking(x => x.ShareholderId == id)
                .Join(_userProfileRepository.GetAllAsNoTracking(),
                x => x.UserProfileId, y => y.UserProfileId, (x, y) => new ShareholderSearchViewModel
                {
                    Email = y.Email,
                    Id = x.ShareholderId,
                    Name = $"{y.FirstName} {y.LastName}",
                    ShareholderTypeCode = x.ShareholderTypeCode

                }).FirstOrDefault();
            return query;
        }
        #endregion

        #region Get By UserId        
        /// <summary>
        /// Gets the by user identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<List<ShareholderDetailModel>> GetByUserId(Guid id)
        {
            var result = _shareholderRepository.GetManyAsNoTracking(x => x.UserProfileId == id)
                        .Select(x => new ShareholderDetailModel
                        {
                            CompanyId = x.CompanyId,
                            ShareholderId = x.ShareholderId,
                            ShareholderType = x.ShareholderTypeCode,
                            UserProfileId = x.UserProfileId
                        }).ToList();
            return result;
        }
        #endregion

        #region Search        
        /// <summary>
        /// Searches the shareholder.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<List<ShareholderSearchViewModel>> SearchShareholder(ShareholderSearchModel model)
        {
            const string defaultSort = "Name ASC";
            var sortType = model.IsSortDesc ? "DESC" : "ASC";
            var sortField = ValidateUtils.IsNullOrEmpty(model.SortField)
                ? defaultSort
                : $"{model.SortField} {sortType}";
            var query = _userProfileRepository.GetManyAsNoTracking(x =>
                  (ValidateUtils.IsNullOrEmpty(model.Name) || x.FirstName.ToUpper().Contains(model.Name.ToUpper()))
                  && (ValidateUtils.IsNullOrEmpty(model.Name) || x.LastName.ToUpper().Contains(model.Name.ToUpper())))
                .Select(x => new
                {
                    x.UserProfileId,
                    name = $"{x.FirstName} {x.LastName}",
                    x.Email
                })
                .Join(_shareholderRepository.GetAllAsNoTracking(),
                x => x.UserProfileId, y => y.UserProfileId, (x, y) => new ShareholderSearchViewModel
                {
                    Id = y.ShareholderId,
                    Email = x.Email,
                    Name = x.name,
                    ShareholderTypeCode = y.ShareholderTypeCode
                }).OrderBy(sortField);
            var result = query.Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize);
            return result.ToList();
        }
        #endregion

        #region Update        
        /// <summary>
        /// Updates the shares holder.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">shareholder id {model.Id} not found</exception>
        public async Task<Shareholder> UpdateSharesHolder(ShareHolderUpdateModel model)
        {
            var result = _shareholderRepository.GetById(model.Id);
            if (result == null) throw new EntityNotFoundException($"shareholder id {model.Id} not found");
            result.ShareholderTypeCode = model.ShareholderTypeCode;
            _shareholderRepository.Update(result);
            await _unitOfWork.CommitAsync();
            return result;
        }
        #endregion
    }
}
