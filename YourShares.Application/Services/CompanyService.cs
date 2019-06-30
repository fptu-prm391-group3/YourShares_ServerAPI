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
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<UserProfile> _userRepository;
        private readonly ISharesAccountService _shareAccountService;

        public CompanyService(IUnitOfWork unitOfWork
            , IRepository<Company> companyRepository
            , IRepository<UserProfile> userRepository
            , ISharesAccountService shareAccountService)
        {
            _unitOfWork = unitOfWork;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _shareAccountService = shareAccountService;
        }

        public async Task<CompanyViewModel> CreateCompany(string userId, CompanyCreateModel model)
        {
            if (ValidateUtils.IsNullOrEmpty(userId)) throw new UnauthorizedUser("User id is invalid");
            var company = new Company
            {
                AdminProfileId = Guid.Parse(userId),
                CompanyName = model.CompanyName,
                Address = model.Address,
                Phone = model.Phone,
                Capital = model.Capital,
                TotalShares = model.TotalShares,
            };
            var newCompany = _companyRepository.Insert(company);
            await _unitOfWork.CommitAsync();
            return new CompanyViewModel
            {
                Id = newCompany.Entity.CompanyId,
                AdminUserId = newCompany.Entity.AdminProfileId,
                CompanyName = newCompany.Entity.CompanyName,
                Address = newCompany.Entity.Address,
                Phone = newCompany.Entity.Phone,
                Capital = newCompany.Entity.Capital,
                TotalShares = newCompany.Entity.TotalShares,
                OptionPoll = newCompany.Entity.OptionPollAmount
            };
        }

        public async Task<bool> UpdateCompany(CompanyUpdateModel model)
        {
            // TODO check editable permission with user id
            var company = _companyRepository.GetById(model.CompanyId);
            if (company == null) throw new EntityNotFoundException($"Company id {model.CompanyId} not found");
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

        public async Task<List<CompanyViewSearchModel>> SearchCompany(string userId, CompanySearchModel model)
        {
            if (ValidateUtils.IsNullOrEmpty(userId)) throw new UnauthorizedUser("User id is invalid");
            const string defaultSort = "CompanyName ASC";
            var sortType = model.IsSortDesc ? "DESC" : "ASC";
            var sortField = ValidateUtils.IsNullOrEmpty(model.SortField)
                ? defaultSort
                : $"{model.SortField} {sortType}";
            var query = _companyRepository.GetManyAsNoTracking(x =>
                    (ValidateUtils.IsNullOrEmpty(model.CompanyName)
                    || x.CompanyName.ToUpper().Contains(model.CompanyName.ToUpper()))
                    && x.AdminProfileId == Guid.Parse(userId)
                ).Select(x => new CompanyViewSearchModel
                {
                    Address = x.Address,
                    Phone = x.Phone,
                    Capital = x.Capital,
                    CompanyId = x.CompanyId,
                    CompanyName = x.CompanyName,
                    OptionPoll = x.OptionPollAmount,
                    TotalShares = x.TotalShares,
                }).OrderBy(sortField);
            var result = query.Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize);
            return result.ToList();
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

        public async Task<bool> IncreaseOptionPool(CompanyIncreaseOptionPoolMode model)
        {
            var company = _companyRepository.GetById(model.CompanyId);
            if (company == null) throw new EntityNotFoundException($"Company id {model.CompanyId} not found");

            // TODO save to round data
            company.OptionPollAmount = company.OptionPollAmount + model.sharesAmount;
            company.TotalShares = company.TotalShares + model.sharesAmount;

            _companyRepository.Update(company);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task AddOptionPoolToSharesholder(CompanyAddOptionPoolToShareholderModel model, Guid CompanyId,Guid SharesholerId)
        {
            var company = _companyRepository.GetById(CompanyId);
            if (company == null) throw new EntityNotFoundException($"Company id {CompanyId} not found");
            company.OptionPollAmount = company.OptionPollAmount - model.RestrictedAmount;
            await _shareAccountService.AddRestrictedShares(SharesholerId, model.RestrictedAmount,model);
        }
    }
}