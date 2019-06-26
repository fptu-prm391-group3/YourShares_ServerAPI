using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourShares.Domain.Models;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("api/shareholders")]
    [Produces("application/json")]
    [Authorize(Roles = "Deochoaivao")]
    public class ShareholderController : ControllerBase
    {
        /// <summary>
        /// Find shareholder specified by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public Task<Shareholder> GetShareholderById([FromRoute] string id)
        {
            return null;
        }
        
        /// <summary>
        /// Search shareholder
        /// </summary>
        /// <param name="shareholder"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<List<Shareholder>> SearchShareholder([FromBody] string shareholder)
        {
            return null;
        }

        /// <summary>
        /// Add a user as shareholder of company
        /// </summary>
        /// <param name="shareholder"></param>
        /// <returns></returns>
        [HttpPost]
        public Task AddUserAsShareholder([FromBody] string shareholder, string company)
        {
            return null;
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