using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourShares.Application.Interfaces;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
using YourShares.Data.Interfaces;
using YourShares.Domain.ApiResponse;
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

        public async Task<string> CreateCompany(CompanyCreateModel model)
        {
            var company = new Company
            {
                CompanyName = model.CompanyName,
                Address = model.Address,
                Capital = model.Capital,
                TotalShares = model.TotalShares,
                OptionPoll = model.OptionPoll
            };
            _companyRepository.Insert(company);
            await _unitOfWork.CommitAsyn();
            return ApiResponse.Ok();
        }

        public async Task<string> GetAllCompany()
        {
            var test = _companyRepository.GetAll();
            int count = await test.CountAsync();
            return ApiResponse.Ok(test, count);
        }

        public async Task<string> UpdateCompany(CompanyUpdateModel model)
        {
            var company = _companyRepository.GetById(model.CompanyId);
            if (company == null)
            {
                return ApiResponse.Error(404, "Company Not Exits");
            }

            company.CompanyName = model.CompanyName;
            company.Address = model.Address;
            company.Capital = model.Capital;
            company.TotalShares = model.TotalShares;
            company.OptionPoll = model.OptionPoll;
            _companyRepository.Insert(company);
            await _unitOfWork.CommitAsyn();
            return ApiResponse.Ok();
        }

        public async Task<string> SearchCompany(CompanySearchModel model)
        {
            string defaultSort = "CompanyName ASC";
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
                    && (ValidateUtils.IsNullOrEmpty(model.Capital) || x.Capital.ToUpper().Contains(model.Capital.ToUpper()))
                    && (ValidateUtils.IsNullOrEmpty(model.CompanyName) || x.CompanyName.ToUpper().Contains(model.CompanyName.ToUpper()))
                ), x => x.Id, y => y.AdminId, (x, y) => new CompanyViewSearchModel
                {
                    AdminUserName = x.UserName,
                    Address = y.Address,
                    Capital = y.Capital,
                    CompanyId = y.Id,
                    CompanyName = y.CompanyName,
                    OptionPoll = y.OptionPoll,
                    TotalShares = y.TotalShares
                }).OrderBy(sortField);
            var count = await query.CountAsync();
            var result = query.Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToList();
            return ApiResponse.Ok(result, count);
        }

        public async Task<string> GetById(Guid id)
        {
            var company = _companyRepository.GetById(id);
            return company == null ? ApiResponse.Error(404, "Company Not Exits") : ApiResponse.Ok(company, 1);
        }

        public async Task<string> DeleteById(Guid id)
        {
            var company = _companyRepository.GetById(id);
            if (company == null)
            {
                return ApiResponse.Error(404, "Company Not Exits");
            }

            _companyRepository.Delete(company);
            await _unitOfWork.CommitAsyn();
            return ApiResponse.Ok();
        }
    }
}