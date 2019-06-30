using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourShares.Application.Interfaces;
using YourShares.Domain.Models;
using YourShares.RestApi.ApiResponse;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("/api/round-investors")]
    [Produces("application/json")]
    [Authorize]
    public class RoundInvestorController
    {
        private readonly IRoundInvestorService _roundInvestorService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="roundInvestorService"></param>
        public RoundInvestorController(IRoundInvestorService roundInvestorService)
        {
            _roundInvestorService = roundInvestorService;
        }

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
    }
}