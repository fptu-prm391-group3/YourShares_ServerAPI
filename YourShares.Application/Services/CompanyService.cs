using System;
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
using YourShares.RestApi.Models;

namespace YourShares.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserProfile> _userRepository;

        public CompanyService(IUnitOfWork unitOfWork
            , IRepository<Company> companyRepository
            , IRepository<UserProfile> userRepository)
        {
            _unitOfWork = unitOfWork;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
        }

        public async Task<CompanyViewModel> CreateCompany(CompanyCreateModel model)
        {
            var company = new Company
            {
                AdminProfileId = Guid.Parse(model.AdminUserId),
                CompanyName = model.CompanyName,
                Address = model.Address,
                Phone = model.Phone,
                Capital = model.Capital,
                TotalShares = model.TotalShares,
                OptionPollAmount = model.OptionPoll
            };
            _companyRepository.Insert(company);
            await _unitOfWork.CommitAsync();
            return new CompanyViewModel
            {
                AdminUserId = Guid.Parse(model.AdminUserId),
                CompanyName = model.CompanyName,
                Address = model.Address,
                Phone = model.Phone,
                Capital = model.Capital,
                TotalShares = model.TotalShares,
                OptionPoll = model.OptionPoll
            };
        }

        public async Task<bool> UpdateCompany(CompanyUpdateModel model)
        {
            var company = _companyRepository.GetById(model.CompanyId);
            company.CompanyName = model.CompanyName;
            company.Address = model.Address;
            company.Phone = model.Phone;
            company.Capital = model.Capital;
            company.TotalShares = model.TotalShares;
            company.OptionPollAmount = model.OptionPoll;
            _companyRepository.Update(company);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<IQueryable<CompanyViewSearchModel>> SearchCompany(CompanySearchModel model)
        {
            const string defaultSort = "CompanyName ASC";
            var sortType = model.IsSortDesc ? "DESC" : "ASC";
            var sortField = ValidateUtils.IsNullOrEmpty(model.SortField)
                ? defaultSort
                : $"{model.SortField} {sortType}";
            var query = _userRepository.GetManyAsNoTracking(x =>
                    ValidateUtils.IsNullOrEmpty(model.AdminEmail) || x.Email == model.AdminEmail)
                .Select(x => new
                {
                    x.UserProfileId,
                    x.Email
                })
                .Join(_companyRepository.GetManyAsNoTracking(x =>
                    (ValidateUtils.IsNullOrEmpty(model.Address) ||
                     x.Address.ToUpper().Contains(model.AdminEmail.ToUpper()))
                    && (model.Capital <= 0 || x.Capital == model.Capital)
                    && (ValidateUtils.IsNullOrEmpty(model.CompanyName) ||
                        x.CompanyName.ToUpper().Contains(model.CompanyName.ToUpper()))
                ), x => x.UserProfileId, y => y.AdminProfileId, (x, y) => new CompanyViewSearchModel
                {
                    AdminEmail = x.Email,
                    Address = y.Address,
                    Phone = y.Phone,
                    Capital = y.Capital,
                    CompanyId = y.CompanyId,
                    CompanyName = y.CompanyName,
                    OptionPoll = y.OptionPollAmount,
                    TotalShares = y.TotalShares
                }).OrderBy(sortField);
            var result = query.Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize);
            return result;
        }

        public async Task<CompanyViewModel> GetById(Guid id)
        {
            var result = _companyRepository.GetById(id);
            if (result == null) throw new EntityNotFoundException($"Company id {id} not found");
            return new CompanyViewModel
            {
                Id = result.CompanyId,
                AdminUserId = result.AdminProfileId,
                CompanyName = result.CompanyName,
                Phone = result.Phone,
                Address = result.Address,
                Capital = result.Capital,
                OptionPoll = result.OptionPollAmount,
                TotalShares = result.TotalShares
            };
        }

        public async Task<bool> DeleteById(Guid id)
        {
            var company = _companyRepository.GetById(id);
            _companyRepository.Delete(company);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}