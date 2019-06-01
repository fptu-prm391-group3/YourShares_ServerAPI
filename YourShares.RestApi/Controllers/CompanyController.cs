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

        public CompanyController(ICompanyService companyService)
        {
            _customerAppService = companyService;
        }
        [HttpGet]
        [Route("search")]
        public async Task<string> SearchCompany([FromQuery] CompanySearchModel model)
        {
            return await _customerAppService.SearchCompany(model);
        }

        [HttpGet]
        public async Task<string> GetAllCompany()
        {
            return await _customerAppService.GetAllCompany();
        }
        

        [HttpGet]
        [Route("{id}")]
        public async Task<string> GetCompanyById(Guid Id)
        {
            return await _customerAppService.GetCompanyById(Id);
        }

        [HttpPost]
        public async Task<string> CreateCompany([FromBody]CompanyCreateModel model)
        {
            return await _customerAppService.CreateCompany(model);
        }

        [HttpPut]
        public async Task<string> UpdateCompany([FromBody]CompanyUpdateModel model)
        {
            return await _customerAppService.UpdateCompany(model);
        }


        [HttpDelete]
        public async Task<string> DeleteCompanyById(Guid Id)
        {
            return await _customerAppService.DeleteById(Id);
        }
    }
}