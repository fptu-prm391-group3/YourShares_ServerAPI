using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourShares.Application.Interfaces;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
using YourShares.RestApi.ApiResponse;

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
        ///     Gets User specified by its identifier.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/user/{id}")]
        public async Task<ResponseModel<UserViewDetailModel>> GetUserById([FromRoute] Guid id)
        {
            var result = await _userService.GetById(id);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<UserViewDetailModel>().Success()
                .Data(result)
                .Count(1)
                .build();
        }

        /// <summary>
        ///     Search User by Email, Phone, Name.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/users")]
        //[Authorize(Roles = "Admin")]
        public async Task<ResponseModel<IQueryable<UserSearchViewModel>>> SearchUser(
            [FromQuery] UserSearchModel model)
        {
            var result = await _userService.SearchUser(model);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<IQueryable<UserSearchViewModel>>().Success()
                .Data(result)
                .Count(result.Count())
                .build();
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