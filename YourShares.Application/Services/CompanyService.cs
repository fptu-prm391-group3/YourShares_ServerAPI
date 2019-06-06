using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
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
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<Administrator> _administratorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IUnitOfWork unitOfWork
            , IRepository<Company> companyRepository
            , IRepository<Administrator> administratorRepository)
        {
            _unitOfWork = unitOfWork;
            _companyRepository = companyRepository;
            _administratorRepository = administratorRepository;
        }

        public async Task<CompanyViewModel> CreateCompany(CompanyCreateModel model)
        {
            var company = new Company
            {
                AdminId = Guid.Parse(model.AdminId),
                Name = model.CompanyName,
                Address = model.Address,
                Phone = model.Phone,
                Capital = model.Capital,
                TotalShare = model.TotalShares,
                OptionPoll = model.OptionPoll
            };
            _companyRepository.Insert(company);
            await _unitOfWork.CommitAsync();
            // TODO Response the created company id
            return new CompanyViewModel
            {
                AdminId = Guid.Parse(model.AdminId),
                Name = model.CompanyName,
                Address = model.Address,
                Phone = model.Phone,
                Capital = model.Capital,
                TotalShare = model.TotalShares,
                OptionPoll = model.OptionPoll
            };
        }

        public async Task<bool> UpdateCompany(CompanyUpdateModel model)
        {
            var company = _companyRepository.GetById(model.Id);
            if (company == null)
            {
                return false;
            }

            company.Name = model.CompanyName;
            company.Address = model.Address;
            company.Phone = model.Phone;
            company.Capital = model.Capital;
            company.TotalShare = model.TotalShares;
            company.OptionPoll = model.OptionPoll;
            _companyRepository.Insert(company);
            // TODO Handle fail commit
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<List<CompanyViewSearchModel>> SearchCompany(CompanySearchModel model)
        {
            const string defaultSort = "CompanyName ASC";
            string sortType = model.IsSortDesc ? "DESC" : "ASC";
            string sortField = ValidateUtils.IsNullOrEmpty(model.SortField)
                ? defaultSort
                : $"{model.SortField} {sortType}";
            var query = _administratorRepository.GetManyAsNoTracking(x =>
                    (ValidateUtils.IsNullOrEmpty(model.AdminUserName) || x.UserName == model.AdminUserName))
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                })
                .Join(_companyRepository.GetManyAsNoTracking(x =>
                    (ValidateUtils.IsNullOrEmpty(model.Address) || x.Address.ToUpper().Contains(model.AdminUserName.ToUpper()))
                    && (model.Capital <= 0 || x.Capital == model.Capital)
                    && (ValidateUtils.IsNullOrEmpty(model.CompanyName) || x.Name.ToUpper().Contains(model.CompanyName.ToUpper()))
                ), x => x.Id, y => y.AdminId, (x, y) => new CompanyViewSearchModel
                {
                    AdminUserName = x.UserName,
                    Address = y.Address,
                    Phone = y.Phone,
                    Capital = y.Capital,
                    Id = y.Id,
                    CompanyName = y.Name,
                    OptionPoll = y.OptionPoll,
                    TotalShares = y.TotalShare
                }).OrderBy(sortField);
            var result = query.Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToList();
            return result;
        }

        public async Task<CompanyViewModel> GetById(Guid id)
        {
            var result = _companyRepository.GetById(id);
            return new CompanyViewModel
            {
                Id = result.Id,
                AdminId = result.AdminId,
                Name = result.Name,
                Phone = result.Phone,
                Address = result.Address,
                Capital = result.Capital,
                OptionPoll = result.OptionPoll,
                TotalShare = result.TotalShare
            };
        }

        public async Task<bool> DeleteById(Guid id)
        {
            var company = _companyRepository.GetById(id);
            if (company == null)
            {
                return false;
            }

            _companyRepository.Delete(company);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}