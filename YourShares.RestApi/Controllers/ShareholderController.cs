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

        public ShareholderController(IShareholderService shareholderService)
        {
            _shareholderService = shareholderService;
        }
        /// <summary>
        /// Find shareholder specified by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<ShareholderSearchViewModel> GetShareholderById([FromRoute] Guid id)
        {
            return await _shareholderService.GetById(id);
        }
        
        /// <summary>
        /// Search shareholder
        /// </summary>
        /// <param name="shareholder"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseModel<List<ShareholderSearchViewModel>>> SearchShareholder([FromQuery] ShareholderSearchModel model)
        {
            var result = await _shareholderService.SearchShareholder(model);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<List<ShareholderSearchViewModel>>().Success()
                .Data(result)
                .Count(result.Count)
                .build();
        }

        /// <summary>
        /// Add a user as shareholder of company
        /// </summary>
        /// <param name="shareholder"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> AddUserAsShareholder([FromBody] ShareHolderAddUserModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _shareholderService.AddUserAsShareHolder(model, userId);
        }

        /// <summary>
        /// Remove shareholder from company
        /// </summary>
        /// <param name="shareholder"></param>
        /// <returns></returns>
        [HttpDelete]
        public Task RemoveShareholderFromCompany([FromBody] string shareholder)
        {
            return null;
        }

    }
}