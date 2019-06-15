using System;
using System.Linq;
using System.Threading.Tasks;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;

namespace YourShares.Application.Interfaces
{
    public interface ICompanyService
    {
        /// <summary>
        ///     Searches the company.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<IQueryable<CompanyViewSearchModel>> SearchCompany(CompanySearchModel model);

        /// <summary>
        ///     Create the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<CompanyViewModel> CreateCompany(CompanyCreateModel model);

        /// <summary>
        ///     Update the company information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<bool> UpdateCompany(CompanyUpdateModel model);

        /// <summary>
        ///     Get the company by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<CompanyViewModel> GetById(Guid id);

        /// <summary>
        ///     Delete company by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<bool> DeleteById(Guid id);
    }
}