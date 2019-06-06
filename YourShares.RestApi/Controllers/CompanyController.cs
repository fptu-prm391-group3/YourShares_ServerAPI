using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YourShares.Application.Interfaces;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
using YourShares.Domain.Models;
using YourShares.RestApi.ApiResponse;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("/api/companies")]
    [Produces("application/json")]
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
        public async Task<Response<CompanyViewModel>> GetCompanyById([FromRoute]Guid id)
        {
            var result = await _customerAppService.GetById(id);
            if (result != null)
            {
                Response.StatusCode = (int) HttpStatusCode.OK;
                return ApiResponse.ApiResponse.Ok(result, 1);
            }
            Response.StatusCode = (int) HttpStatusCode.NotFound;
            // TODO throw exception if not found
            return null;
        }
        
        /// <summary>
        /// Search company by Admin, Name, Address, Capital.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<CompanyViewSearchModel>>> SearchCompany([FromQuery] CompanySearchModel model)
        {
            var result = await _customerAppService.SearchCompany(model);
            if (result != null)
            {
                Response.StatusCode = (int) HttpStatusCode.OK;
                return ApiResponse.ApiResponse.Ok(result, result.Count);
            }
            Response.StatusCode = (int) HttpStatusCode.BadRequest;
            return null;
        }
        
        /// <summary>
        /// Creates the company.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<CompanyViewModel>> CreateCompany([FromBody]CompanyCreateModel model)
        {
            var createdResult = await _customerAppService.CreateCompany(model);
            if (createdResult != null)
            {
                Response.StatusCode = (int) HttpStatusCode.Created;
                return ApiResponse.ApiResponse.Ok(createdResult, 1);
            }

            Response.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
            return null;
        }

        /// <summary>
        /// Updates the company with details in the request body.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateCompany([FromBody]CompanyUpdateModel model)
        {
            var updated = await _customerAppService.UpdateCompany(model);
            if (updated)
            {
                Response.StatusCode = (int) HttpStatusCode.OK;
            }

            Response.StatusCode = (int) HttpStatusCode.BadRequest;
        }


        /// <summary>
        /// Delete a company specified by its identifier.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteCompanyById([FromRoute]Guid id)
        {
            var deleted = await _customerAppService.DeleteById(id);
            if (deleted)
            {
                Response.StatusCode = (int) HttpStatusCode.Accepted;
            }

            Response.StatusCode = (int) HttpStatusCode.BadRequest;
        }
    }
}