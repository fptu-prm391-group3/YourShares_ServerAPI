using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourShares.Application.Interfaces;
using YourShares.Domain.Models;
using YourShares.RestApi.ApiResponse;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    [Produces("application/json")]
    [Authorize]
    public class TransactionController: ControllerBase
    {
        private readonly ITransactionService _transactionService;

        #region Constructor
        /// <summary>
        ///     Initializes a new instance of the <see cref="CompanyController" /> class.
        /// </summary>
        /// <param name="companyService">The company service.</param>
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        #endregion

        #region Gets the Transaction by id.
        /// <summary>
        /// Gets the Transaction by id.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<ResponseModel<Transaction>> GetById([FromRoute] Guid id)
        {
            var result = await _transactionService.GetById(id);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<Transaction>().Success()
                .Data(result)
                .build();
        }
        #endregion


        /// <summary>
        /// Gets the list transaction by shares account id.
        /// </summary>
        /// <param name="id">The.</param>
        /// <returns></returns>
        [Route("share-accounts/{id}")]
        [HttpGet]
        public async Task<ResponseModel<List<Transaction>>> GetBySharesAccountId([FromRoute] Guid id)
        {
            var result = await _transactionService.GetBySharesAccountId(id);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<List<Transaction>>().Success()
                .Data(result)
                .Count(result.Count)
                .build();
        }
    }
}