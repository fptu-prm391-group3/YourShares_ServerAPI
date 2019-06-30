using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourShares.Application.Interfaces;
using YourShares.Application.ViewModels;
using YourShares.RestApi.ApiResponse;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("api/share-accounts")]
    [Produces("application/json")]
    [Authorize]
    public class ShareAccountController : ControllerBase
    {
        private readonly ISharesAccountService _sharesAccountService;
        public ShareAccountController(ISharesAccountService sharesAccountService)
        {
            _sharesAccountService = sharesAccountService;
        }

        /// <summary>
        /// Views the shares account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseModel<List<SharesAccountViewModel>>> ViewSharesAccount([FromQuery] ShareAccountGetDetailModel model)
        {
            var result = await _sharesAccountService.ViewSharesAccount(model);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<List<SharesAccountViewModel>>().Success()
                .Data(result)
                .Count(result.Count)
                .build();
        }

        /// <summary>
        /// Views the shares account.
        /// </summary>
        /// <param name="companyId">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public async Task<ResponseModel<List<ShareAccountViewAllModel>>> ViewAllSharesAccount([FromQuery] Guid companyId)
        {
            var result = await _sharesAccountService.ViewAllSharesAccount(companyId);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<List<ShareAccountViewAllModel>>().Success()
                .Data(result)
                .Count(result.Count)
                .build();
        }
    }
}