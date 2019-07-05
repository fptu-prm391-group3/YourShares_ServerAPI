using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
using YourShares.Domain.Models;

namespace YourShares.Application.Interfaces
{
    public interface ICompanyService
    {
        /// <summary>
        ///     Searches the company.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<List<Company>> SearchCompany(string userId, CompanySearchModel model);

        /// <summary>
        ///     Create the specified model.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<Company> CreateCompany(string userId, CompanyCreateModel model);

        /// <summary>
        ///     Update the company information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> UpdateCompany(Guid id, CompanyUpdateModel model);

        /// <summary>
        ///     Get the company by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Company> GetById(Guid id);

        Task<List<Company>> GetCompaniesByAdmin(Guid id);

        /// <summary>
        ///     Delete company by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<bool> DeleteById(Guid id);

        Task<bool> IncreaseOptionPool(CompanyIncreaseOptionPoolMode model);

        Task AddOptionPoolToShareholder(CompanyAddOptionPoolToShareholderModel model,Guid companyId,Guid shareholderId);
    }
}