using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourShares.Application.Exceptions;
using YourShares.Application.Interfaces;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;
using YourShares.Domain.Util;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.Internal;

namespace YourShares.Application.Services
{
    public class ShareholderService : IShareholderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Shareholder> _shareholderRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<UserProfile> _userProfileRepository;

        public ShareholderService(IUnitOfWork unitOfWork, IRepository<Shareholder> shareholderRepository,
                                 IRepository<Company> companyRepository, IRepository<UserProfile> userProfileRepository)
        {
            _unitOfWork = unitOfWork;
            _shareholderRepository = shareholderRepository;
            _companyRepository = companyRepository;
            _userProfileRepository = userProfileRepository;
        }

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
    }
}
