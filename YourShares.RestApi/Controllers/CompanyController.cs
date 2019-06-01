using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YourShares.Application.Interfaces;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _customerAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyController"/> class.
        /// </summary>
        /// <param name="companyService">The company service.</param>
        public CompanyController(ICompanyService companyService)
        {
            _customerAppService = companyService;
        }
        
        /// <summary>
        /// Gets company specified by its identifier.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<string> GetCompanyById([FromRoute]Guid id)
        {
            return await _customerAppService.GetById(id);
        }
        
        /// <summary>
        /// Gets all company.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> GetAllCompany()
        {
            return await _customerAppService.GetAllCompany();
        }
        
        /// <summary>
        /// Search company by Admin, Name, Address, Capital.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("search")]
        public async Task<string> SearchCompany([FromQuery] CompanySearchModel model)
        {
            return await _customerAppService.SearchCompany(model);
        }
        
        /// <summary>
        /// Creates the company.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CreateCompany([FromBody]CompanyCreateModel model)
        {
            return await _customerAppService.CreateCompany(model);
        }

        /// <summary>
        /// Updates the company with details in the request body.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<string> UpdateCompany([FromBody]CompanyUpdateModel model)
        {
            return await _customerAppService.UpdateCompany(model);
        }


        /// <summary>
        /// Delete a company specified by its identifier.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<string> DeleteCompanyById([FromRoute]Guid id)
        {
            return await _customerAppService.DeleteById(id);
        }
    }
}