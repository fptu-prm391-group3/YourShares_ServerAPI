using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourShares.Application.Interfaces;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
using YourShares.RestApi.ApiResponse;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("/api/companies")]
    [Produces("application/json")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CompanyController" /> class.
        /// </summary>
        /// <param name="companyService">The company service.</param>
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>
        ///     Gets company specified by its identifier.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ResponseModel<CompanyViewModel>> GetCompanyById([FromRoute] Guid id)
        {
            var result = await _companyService.GetById(id);
            Response.StatusCode = (int) HttpStatusCode.OK;
            return new ResponseBuilder<CompanyViewModel>().Success()
                .Data(result)
                .Count(1)
                .build();
        }

        /// <summary>
        ///     Search company by CompanyName.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<ResponseModel<IQueryable<CompanyViewSearchModel>>> SearchCompany(
            [FromQuery] CompanySearchModel model)
        {
            var result = await _companyService.SearchCompany(model);
            Response.StatusCode = (int) HttpStatusCode.OK;
            return new ResponseBuilder<IQueryable<CompanyViewSearchModel>>().Success()
                .Data(result)
                .Count(result.Count())
                .build();
        }

        /// <summary>
        ///     Creates the company.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseModel<CompanyViewModel>> CreateCompany([FromBody] CompanyCreateModel model)
        {
            var createdResult = await _companyService.CreateCompany(model);
            Response.StatusCode = (int) HttpStatusCode.Created;
            return new ResponseBuilder<CompanyViewModel>().Success()
                .Data(createdResult)
                .build();
        }

        /// <summary>
        ///     Updates the company with details in the request body.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateCompany([FromBody] CompanyUpdateModel model)
        {
            await _companyService.UpdateCompany(model);
            Response.StatusCode = (int) HttpStatusCode.OK;
        }


        /// <summary>
        ///     Delete a company specified by its identifier.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteCompanyById([FromRoute] Guid id)
        {
            await _companyService.DeleteById(id);
            Response.StatusCode = (int) HttpStatusCode.Accepted;
        }
    }
}