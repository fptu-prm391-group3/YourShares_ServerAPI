using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YourShares.Application;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _customerAppService;
        public CompanyController(ICompanyService companyService)
        {
            _customerAppService = companyService;
        }
        // GET api/values
        [HttpGet]
        [Route("api/demo")]
        public async Task<string> Get()
        {
            string a = await _customerAppService.GetDetail();
            return a;
        }
    }
}