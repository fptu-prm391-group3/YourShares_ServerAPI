using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    [Produces("application/json")]
    [Authorize(Roles = "Deochoaivao")]
    public class TransactionController
    {
        /// <summary>
        /// Get transaction by its identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public Task GetById([FromRoute] string id)
        {
            return null;
        }

        /// <summary>
        /// Get all transaction by share account
        /// </summary>
        /// <param name="shareAccountId"></param>
        /// <returns></returns>
        [Route("share-accounts/{id}")]
        [HttpGet]
        public Task GetTransactionByShareAccount([FromRoute] string shareAccountId)
        {
            return null;
        }
        
    }
}