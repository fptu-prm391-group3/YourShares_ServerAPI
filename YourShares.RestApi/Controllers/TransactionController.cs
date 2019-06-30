using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    [Produces("application/json")]
    [Authorize]
    public class TransactionController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public async Task GetById([FromRoute] Guid id)
        {
            // TODO 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("share-accounts/{id}")]
        [HttpGet]
        public async Task GetBySharesAccountId([FromRoute] Guid id)
        {
            // TODO
        }
    }
}