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

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sharesAccountService"></param>
        public ShareAccountController(ISharesAccountService sharesAccountService)
        {
            _sharesAccountService = sharesAccountService;
        }
        #endregion

        #region Get by Id       
        /// <summary>
        /// Gets the SharesAccount by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<ResponseModel<SharesAccountDetailModel>> GetById([FromRoute] Guid id)
        {
            var result = await _sharesAccountService.GetById(id);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<SharesAccountDetailModel>().Success()
                .Data(result)
                .build();
        }
        #endregion

        #region Gets the SharesAccount by shareholderId.
        /// <summary>
        /// Gets the SharesAccount by shareholderId.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("shareholders/{id}")]
        [HttpGet]
        public async Task<ResponseModel<List<SharesAccountDetailModel>>> GetByShareholderId([FromRoute] Guid id)
        {
            var result = await _sharesAccountService.GetByShareholderId(id);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<List<SharesAccountDetailModel>>().Success()
                .Data(result)
                .build();
        }
        #endregion

        #region View Shares Account Of User In Company (*)
        /// <summary>
        /// Views the shares account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseModel<List<SharesAccountViewModel>>> ViewSharesAccountOfUserInCompany([FromQuery] ShareAccountGetDetailModel model)
        {
            var result = await _sharesAccountService.ViewSharesAccountOfUserInCompany(model);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<List<SharesAccountViewModel>>().Success()
                .Data(result)
                .Count(result.Count)
                .build();
        }
        #endregion
        
        #region View all shares account of company (*)
        /// <summary>
        /// Views all shares account in a company
        /// </summary>
        /// <param name="companyId">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public async Task<ResponseModel<List<ShareAccountViewAllModel>>> ViewAllSharesAccountOfCompany([FromQuery] Guid companyId)
        {
            var result = await _sharesAccountService.ViewAllSharesAccountOfCompany(companyId);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<List<ShareAccountViewAllModel>>().Success()
                .Data(result)
                .Count(result.Count)
                .build();
        }
        #endregion
        
    }
}