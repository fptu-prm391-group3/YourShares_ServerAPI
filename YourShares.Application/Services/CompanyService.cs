using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Company> CreateCompany(string userId, CompanyCreateModel model)
        {
            if (ValidateUtils.IsNullOrEmpty(userId)) throw new UnauthorizedUser();
            var company = new Company
            {
                AdminProfileId = Guid.Parse(userId),
                CompanyName = model.CompanyName,
                CompanyDescription = model.CompanyDescription,
                Address = model.Address,
                Phone = model.Phone,
                Capital = model.Capital,
                TotalShares = model.TotalShares,
            };
            var inserted = _companyRepository.Insert(company).Entity;
            await _unitOfWork.CommitAsync();
            return inserted;
        }

        public async Task<bool> UpdateCompany(Guid id, CompanyUpdateModel model)
        {
            // TODO check editable permission with user id
            var company = _companyRepository.GetById(id);
            if (company == null) throw new EntityNotFoundException($"Company id {id} not found");
            company.CompanyName = model.CompanyName;
            company.CompanyDescription = model.CompanyDescription;
            company.Address = model.Address;
            company.Phone = model.Phone;
            company.Capital = model.Capital;
            company.TotalShares = model.TotalShares;
            company.OptionPollAmount = model.OptionPoll;
            _companyRepository.Update(company);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<List<Company>> SearchCompany(string userId, CompanySearchModel model)
        {
            if (ValidateUtils.IsNullOrEmpty(userId)) throw new UnauthorizedUser();
            const string defaultSort = "CompanyName ASC";
            var sortType = model.IsSortDesc ? "DESC" : "ASC";
            var sortField = ValidateUtils.IsNullOrEmpty(model.SortField)
                ? defaultSort
                : $"{model.SortField} {sortType}";
            var query = _companyRepository.GetManyAsNoTracking(x =>
                    (ValidateUtils.IsNullOrEmpty(model.CompanyName)
                    || x.CompanyName.ToUpper().Contains(model.CompanyName.ToUpper()))
                    && x.AdminProfileId == Guid.Parse(userId)
                ).Select(x => new Company
                {
                    Address = x.Address,
                    Phone = x.Phone,
                    Capital = x.Capital,
                    CompanyId = x.CompanyId,
                    AdminProfileId = x.AdminProfileId,
                    CompanyName = x.CompanyName,
                    CompanyDescription = x.CompanyDescription,
                    OptionPollAmount = x.OptionPollAmount,
                    TotalShares = x.TotalShares
                })
                .OrderBy(sortField);
            var result = query.Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize);
            return result.ToList();
        }

        public async Task<Company> GetById(Guid id)
        {
            var result = _companyRepository.GetById(id);
            if (result == null) throw new EntityNotFoundException($"Company id {id} not found");
            return result;
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
            
            company.OptionPollAmount += model.SharesAmount;
            company.TotalShares += model.SharesAmount;

            _companyRepository.Update(company);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task AddOptionPoolToShareholder(CompanyAddOptionPoolToShareholderModel model, Guid CompanyId,Guid SharesholerId)
        {
            var company = _companyRepository.GetById(CompanyId);
            if (company == null) throw new EntityNotFoundException($"Company id {CompanyId} not found");
            company.OptionPollAmount -= model.RestrictedAmount;
            await _shareAccountService.AddRestrictedShares(SharesholerId, model.RestrictedAmount,model);
        }
    }
}