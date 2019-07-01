using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourShares.Application.Interfaces;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
using YourShares.Domain.Models;
using YourShares.RestApi.ApiResponse;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("api/shareholders")]
    [Produces("application/json")]
    [Authorize]
    public class ShareholderController : ControllerBase
    {
        private readonly IShareholderService _shareholderService;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shareholderService"></param>
        public ShareholderController(IShareholderService shareholderService)
        {
            _shareholderService = shareholderService;
        }
        #endregion
        
        #region GetById
        /// <summary>
        /// Find shareholder specified by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<ShareholderSearchViewModel> GetById([FromRoute] Guid id)
        {
            return await _shareholderService.GetById(id);
        }
        #endregion

        #region Gets List Shareholder Detail by company id.
        /// <summary>
        /// Gets List Shareholder Detail by company id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [Route("companies/{id}")]
        [HttpGet]
        public async Task<ResponseModel<List<ShareholderDetailModel>>> GetByCompanyId([FromRoute] Guid id)
        {
            var result = await _shareholderService.GetByCompanyId(id);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<List<ShareholderDetailModel>>().Success()
                .Data(result)
                .Count(result.Count)
                .build();
        }
        #endregion

        #region Gets List Shareholder detail by user id.
        /// <summary>
        /// Gets List Shareholder detail by user id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("users/{id}")]
        [HttpGet]
        public async Task<ResponseModel<List<ShareholderDetailModel>>> GetByUserId([FromRoute] Guid id)
        {
            var result = await _shareholderService.GetByUserId(id);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<List<ShareholderDetailModel>>().Success()
                .Data(result)
                .Count(result.Count)
                .build();
        }
        #endregion

        #region Search
        /// <summary>
        /// Search shareholder by name
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseModel<List<ShareholderSearchViewModel>>> SearchShareholder(
            [FromQuery] ShareholderSearchModel model)
        {
            var result = await _shareholderService.SearchShareholder(model);
            Response.StatusCode = (int) HttpStatusCode.OK;
            return new ResponseBuilder<List<ShareholderSearchViewModel>>().Success()
                .Data(result)
                .Count(result.Count)
                .build();
        }
        #endregion

        #region AddUserAsShareholder
        /// <summary>
        /// Add a user as shareholder of company
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Shareholder> AddUserAsShareholder([FromBody] ShareHolderAddUserModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _shareholderService.AddUserAsShareHolder(model, userId);
        }
        #endregion
        
    }
}