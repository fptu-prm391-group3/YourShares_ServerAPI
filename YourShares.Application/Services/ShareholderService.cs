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
using YourShares.RestApi.Models;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.Internal;

namespace YourShares.Application.Services
{
    public class ShareholderService : IShareholderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Shareholder> _shareholdertRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<UserProfile> _userProfileRepository;

        public ShareholderService(IUnitOfWork unitOfWork, IRepository<Shareholder> shareholdertRepository,
                                 IRepository<Company> companyRepository, IRepository<UserProfile> userProfileRepository)
        {
            _unitOfWork = unitOfWork;
            _shareholdertRepository = shareholdertRepository;
            _companyRepository = companyRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<bool> AddUserAsShareHolder(ShareHolderAddUserModel model, string currentUserId)
        {
            Guid id = Guid.Parse(currentUserId);
            var company = _companyRepository.GetManyAsNoTracking(x => x.AdminProfileId == id && x.CompanyId == model.CompanyId);
            if (company == null) throw new EntityNotFoundException($"Company {model.CompanyId} not found");
            var shareholder = new Shareholder
            {
                CompanyId = model.CompanyId,
                UserId = model.UserId,
                ShareholderTypeCode = model.ShareholderType,
            };
            _shareholdertRepository.Insert(shareholder);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<ShareholderSearchViewModel> GetById(Guid id)
        {
            var result = _shareholdertRepository.GetById(id);
            if (result == null) throw new EntityNotFoundException($"shareholder id {id} not found");
            var query = _shareholdertRepository.GetManyAsNoTracking(x => x.ShareholderId == id)
                .Join(_userProfileRepository.GetAllAsNoTracking(),
                x => x.UserId, y => y.UserProfileId, (x, y) => new ShareholderSearchViewModel
                {
                    Email=y.Email,
                    Id=x.ShareholderId,
                    Name= $"{y.FirstName} {y.LastName}",
                    ShareholderTypeCode=x.ShareholderTypeCode

                }).FirstOrDefault();
            return query;
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
                .Join(_shareholdertRepository.GetAllAsNoTracking(),
                x => x.UserProfileId, y => y.UserId, (x, y) => new ShareholderSearchViewModel
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
