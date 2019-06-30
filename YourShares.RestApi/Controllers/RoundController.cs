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
    [Route("/api/rounds")]
    [Produces("application/json")]
    [Authorize]
    public class RoundController : ControllerBase
    {
        private readonly IRoundService _roundService;

        public RoundController(IRoundService roundService)
        {
            _roundService = roundService;
        }

        /// <summary>
        /// Get round specified by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseModel<Round>> GetById(Guid id)
        {
            var result = await _roundService.GetById(id);
            return new ResponseBuilder<Round>()
                .Success()
                .Data(result)
                .Count(1)
                .build();
        }

        [Route("companies/{id}")]
        [HttpGet]
        public async Task<ResponseModel<List<Round>>> GetByCompanyId(Guid id)
        {
            var result = await _roundService.GetByCompanyId(id);
            return new ResponseBuilder<List<Round>>()
                .Success()
                .Data(result)
                .Count(result.Count)
                .build();
        }
    }
}