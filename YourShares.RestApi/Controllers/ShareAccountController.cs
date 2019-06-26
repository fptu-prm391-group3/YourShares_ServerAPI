using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("api/share-accounts")]
    [Produces("application/json")]
    [Authorize(Roles = "Deochoaivao")]
    public class ShareAccountController : ControllerBase
    {
        /// <summary>
        /// Get share account by its id
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
        /// Create a transaction request and submit to company administrator
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [Route("req")]
        [HttpPost]
        public Task RequestTransaction([FromBody] string transaction)
        {
            return null;
        }

        /// <summary>
        /// Get all transaction of a shareholder
        /// </summary>
        /// <param name="shareholderId"></param>
        /// <returns></returns>
        [Route("shareholders/{id}")]
        [HttpGet]
        public Task GetTransactionByShareholder([FromRoute] string shareholderId)
        {
            return null;
        }
    }
}