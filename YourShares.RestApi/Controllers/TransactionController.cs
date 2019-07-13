using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using YourShares.Application.Interfaces;
using YourShares.Application.ViewModels;
using YourShares.Domain.Models;
using YourShares.RestApi.ApiResponse;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    [Produces("application/json")]
    [Authorize]
    public class TransactionController : ControllerBase
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

        #region Gets the list transaction by shares account id.
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
        #endregion

        #region Request Transaction with another User.
        /// <summary>
        /// Request Transaction with another User.
        /// </summary>
        /// <param name="model">The.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseModel<bool>> RequestTransaction([FromBody] TransactionRequestModel model)
        {
            var result = await _transactionService.RequestTransaction(model);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<bool>().Success()
                .Data(result)
                .Count(1)
                .build();
        }
        #endregion

        #region Handeling Transaction.
        /// <summary>
        /// Handeling Transaction.
        /// </summary>
        /// <param name="model">The.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<ResponseModel<bool>> HandelingTransaction([FromRoute] Guid id, [FromBody] TransactionHandelingModel model)
        {
            var result = await _transactionService.HandelingTransaction(id, model);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<bool>().Success()
                .Data(result)
                .Count(1)
                .build();
        }
        #endregion

    }
}