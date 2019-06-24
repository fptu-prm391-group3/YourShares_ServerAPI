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
            //TODO Generate current user Login
            var currentUserId = Guid.Parse("54eaf8e7-7566-4c8d-a467-ee94e158d975");

            var company = new Company
            {
                AdminProfileId = currentUserId,
                CompanyName = model.CompanyName,
                Address = model.Address,
                Phone = model.Phone,
                Capital = model.Capital,
                TotalShares = model.TotalShares,
                OptionPollAmount = model.OptionPoll
            };
            var newComany = _companyRepository.Insert(company);
            await _unitOfWork.CommitAsync();
            return new CompanyViewModel
            {
                Id = newComany.Entity.CompanyId,
                AdminUserId = newComany.Entity.AdminProfileId,
                CompanyName = newComany.Entity.CompanyName,
                Address = newComany.Entity.Address,
                Phone = newComany.Entity.Phone,
                Capital = newComany.Entity.Capital,
                TotalShares = newComany.Entity.TotalShares,
                OptionPoll = newComany.Entity.OptionPollAmount
            };
        }

        public async Task<bool> UpdateCompany(CompanyUpdateModel model)
        {
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

        public async Task<IQueryable<CompanyViewSearchModel>> SearchCompany(CompanySearchModel model)
        {
            //TODO Generate current user Login
            var currentUserId = Guid.Parse("54eaf8e7-7566-4c8d-a467-ee94e158d975");


            const string defaultSort = "CompanyName ASC";
            var sortType = model.IsSortDesc ? "DESC" : "ASC";
            var sortField = ValidateUtils.IsNullOrEmpty(model.SortField)
                ? defaultSort
                : $"{model.SortField} {sortType}";
            var query = _companyRepository.GetManyAsNoTracking(x =>
                    (ValidateUtils.IsNullOrEmpty(model.CompanyName)
                    || x.CompanyName.ToUpper().Contains(model.CompanyName.ToUpper()))
                    && x.AdminProfileId == currentUserId
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