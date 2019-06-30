using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourShares.Application.Interfaces;
using YourShares.Application.ViewModels;
using YourShares.Domain.Models;
using YourShares.RestApi.ApiResponse;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("/api/round-investors")]
    [Produces("application/json")]
    [Authorize]
    public class RoundInvestorController : ControllerBase
    {
        private readonly IRoundInvestorService _roundInvestorService;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="roundInvestorService"></param>
        public RoundInvestorController(IRoundInvestorService roundInvestorService)
        {
            _roundInvestorService = roundInvestorService;
        }
        #endregion

        #region Get by Id
        /// <summary>
        /// Get Round investor specified by its identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ResponseModel<RoundInvestor>> GetById(Guid id)
        {
            var result = await _roundInvestorService.GetById(id);
            return new ResponseBuilder<RoundInvestor>()
                .Success()
                .Data(result)
                .Count(1)
                .build();
        }
        #endregion

        #region Get by Round Id
        /// <summary>
        /// Get all Round investor of a round specified by round id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("rounds/{id}")]
        public async Task<ResponseModel<List<RoundInvestor>>> GetByRoundId(Guid id)
        {
            var result = await _roundInvestorService.GetByRoundId(id);
            return new ResponseBuilder<List<RoundInvestor>>()
                .Success()
                .Data(result)
                .Count(result.Count)
                .build();
        }
        #endregion

        #region Create round investor
        /// <summary>
        /// Create a round investor, detail in request body
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<RoundInvestor> CreateRoundInvestor(RoundInvestorCreateModel model)
        {
            var result = await _roundInvestorService.InsertRoundInvestor(model);
            Response.StatusCode = (int) HttpStatusCode.Created;
            return result;
        }
        #endregion
    }
}