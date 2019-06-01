using System;
using System.Threading.Tasks;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;

namespace YourShares.Application.Interfaces
{
    public interface ICompanyService
    {
        /// <summary>
        /// Searches the company.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<string> SearchCompany(CompanySearchModel model);

        /// <summary>
        /// Get all company.
        /// </summary>
        /// <returns></returns>
        Task<string> GetAllCompany();

        /// <summary>
        /// Create the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<string> CreateCompany(CompanyCreateModel model);

        /// <summary>
        /// Update the company information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<string> UpdateCompany(CompanyUpdateModel model);

        /// <summary>
        /// Get the company by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<string> GetById(Guid id);

        /// <summary>
        /// Delete company by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<string> DeleteById(Guid id);
    }
}