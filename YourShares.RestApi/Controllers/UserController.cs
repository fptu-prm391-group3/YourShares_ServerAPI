using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourShares.Application.Interfaces;
using YourShares.Application.ViewModels;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Values the tuple.
        /// </summary>
        /// <typeparam name="ICompanyService">The type of the company service.</typeparam>
        /// <returns></returns>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        ///     Updates the user infomation with details in the request body.
        /// </summary>
        /// <param name="model">The UserEditInfoModel.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("/api/user/information")]
        public async Task UpdateInfo([FromBody] UserEditInfoModel model)
        {
            await _userService.UpdateInfo(model);
            Response.StatusCode = (int)HttpStatusCode.OK;
        }

        /// <summary>
        ///     Updates the user email with details in the request body.
        /// </summary>
        /// <param name="model">The UserEditEmailModel.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("/api/user/email")]
        public async Task UpdateInfo([FromBody] UserEditEmailModel model)
        {
            await _userService.UpdateEmail(model);
            Response.StatusCode = (int)HttpStatusCode.OK;
        }
        
    }
}