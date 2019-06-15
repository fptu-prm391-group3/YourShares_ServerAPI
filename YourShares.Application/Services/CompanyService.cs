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
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IUnitOfWork unitOfWork
            , IRepository<Company> companyRepository
            , IRepository<User> userRepository)
        {
            _unitOfWork = unitOfWork;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
        }

        public async Task<CompanyViewModel> CreateCompany(CompanyCreateModel model)
        {
            var company = new Company
            {
                AdminUserId = Guid.Parse(model.AdminUserId),
                CompanyName = model.CompanyName,
                Address = model.Address,
                Phone = model.Phone,
                Capital = model.Capital,
                TotalShare = model.TotalShare,
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
                TotalShare = model.TotalShare,
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
            company.TotalShare = model.TotalShare;
            company.OptionPollAmount = model.OptionPoll;
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
            var query = _userRepository.GetManyAsNoTracking(x =>
                    (ValidateUtils.IsNullOrEmpty(model.AdminUserName) || x.Username == model.AdminUserName))
                .Select(x => new
                {
                    x.UserId,
                    x.Username,
                })
                .Join(_companyRepository.GetManyAsNoTracking(x =>
                    (ValidateUtils.IsNullOrEmpty(model.Address) || x.Address.ToUpper().Contains(model.AdminUserName.ToUpper()))
                    && (model.Capital <= 0 || x.Capital == model.Capital)
                    && (ValidateUtils.IsNullOrEmpty(model.CompanyName) || x.CompanyName.ToUpper().Contains(model.CompanyName.ToUpper()))
                ), x => x.UserId, y => y.AdminUserId, (x, y) => new CompanyViewSearchModel
                {
                    AdminUserName = x.Username,
                    Address = y.Address,
                    Phone = y.Phone,
                    Capital = y.Capital,
                    CompanyId = y.AdminUserId,
                    CompanyName = y.CompanyName,
                    OptionPoll = y.OptionPollAmount,
                    TotalShare = y.TotalShare
                }).OrderBy(sortField);
            var result = query.Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToList();
            return result;
        }

        public async Task<CompanyViewModel> GetById(Guid id)
        {
            var result = _companyRepository.GetById(id);
            if (result == null)
            {
                throw new EntityNotFoundException($"Company id {id} not found");
            }
            return new CompanyViewModel
            {
                Id = result.CompanyId,
                AdminUserId = result.AdminUserId,
                CompanyName = result.CompanyName,
                Phone = result.Phone,
                Address = result.Address,
                Capital = result.Capital,
                OptionPoll = result.OptionPollAmount,
                TotalShare = result.TotalShare
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