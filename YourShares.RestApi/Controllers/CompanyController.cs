using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
    [Authorize]
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
            Response.StatusCode = (int)HttpStatusCode.OK;
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
        public async Task<ResponseModel<List<CompanyViewSearchModel>>> SearchCompany(
            [FromQuery] CompanySearchModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _companyService.SearchCompany(userId, model);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<List<CompanyViewSearchModel>>().Success()
                .Data(result)
                .Count(result.Count)
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var createdResult = await _companyService.CreateCompany(userId, model);
            Response.StatusCode = (int)HttpStatusCode.Created;
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
            Response.StatusCode = (int)HttpStatusCode.OK;
        }

        /// <summary>
        ///     Updates the company with details in the request body.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("/companies/{companyId}/shareholders/{shareholderId}/share-accounts/")]
        public async Task AddOptionPoolToSharesholder([FromBody] CompanyAddOptionPoolToShareholderModel model
                                                       , [FromRoute] Guid companyId
                                                       , [FromRoute] Guid shareholderId)
        {
            await _companyService.AddOptionPoolToSharesholder(model,companyId,shareholderId);
            Response.StatusCode = (int)HttpStatusCode.OK;
        }

        /// <summary>
        ///     Increase OptionPool in company with details in the request body.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("option-pool")]
        public async Task<bool> IncreaseOptionPool([FromBody] CompanyIncreaseOptionPoolMode model)
        {
            var result = await _companyService.IncreaseOptionPool(model);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return result;
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
            Response.StatusCode = (int)HttpStatusCode.Accepted;
        }
    }
}