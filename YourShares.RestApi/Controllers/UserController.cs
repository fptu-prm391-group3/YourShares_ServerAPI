﻿using System;
 using System.Collections.Generic;
 using System.Diagnostics;
 using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YourShares.Application.Interfaces;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
using YourShares.RestApi.ApiResponse;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("/api/users")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="UserController" /> class.
        /// </summary>
        /// <typeparam name="userProfileService">The type of the service.</typeparam>
        /// <returns></returns>
        public UserController(IUserProfileService userProfileService, IConfiguration configuration)
        {
            _userProfileService = userProfileService;
            _configuration = configuration;
        }

        /// <summary>
        ///     Gets User specified by its identifier.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("/api/user/{id}")]
        public async Task<ResponseModel<UserViewDetailModel>> GetUserById([FromRoute] Guid id)
        {
            var result = await _userProfileService.GetById(id);
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
        [Authorize]
        [Route("/api/users")]
        public async Task<ResponseModel<List<UserSearchViewModel>>> SearchUser(
            [FromQuery] UserSearchModel model)
        {
            var result = await _userProfileService.SearchUser(model);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new ResponseBuilder<List<UserSearchViewModel>>().Success()
                .Data(result)
                .Count(result.Count)
                .build();
        }


        /// <summary>
        ///     Updates the user information with details in the request body.
        /// </summary>
        /// <param name="model">The UserEditInfoModel.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("/api/user/information")]
        public async Task UpdateInfo([FromBody] UserEditInfoModel model)
        {
            await _userProfileService.UpdateInfo(model);
            Response.StatusCode = (int)HttpStatusCode.OK;
        }

        /// <summary>
        ///     Updates the user email with details in the request body.
        /// </summary>
        /// <param name="model">The UserEditEmailModel.</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("/api/user/email")]
        public async Task UpdateInfo([FromBody] UserEditEmailModel model)
        {
            await _userProfileService.UpdateEmail(model);
            Response.StatusCode = (int)HttpStatusCode.OK;
        }

        /// <summary>
        /// Register a user account in system.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task Register([FromBody]UserCreateViewModel model)
        {
            await _userProfileService.CreateUserProfile(new UserProfileCreateModel
            {
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                LastName = model.LastName,
                FirstName = model.FirstName
            }, new UserAccountCreateModel
            {
                Email = model.Email,
                Password = model.Password
            });
        }
        
        /// <summary>
        /// Login to system with a system account.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("auth")]
        public async Task<string> LoginWithEmail([FromBody]UserCreateViewModel model)
        {
            var user = await _userProfileService.GetUserByEmail(model.Email);
            if (model.Password == user.PasswordHash)
            {
                var token = BuildToken(user);
                Console.WriteLine(user.Email);
                Console.WriteLine(user.PasswordHash);
                return $"{{\"jwt\":\"{token}\"}}";
            }

            Response.StatusCode = (int) HttpStatusCode.BadRequest;
            return null;
        }
        
        private string BuildToken(UserLoginViewModel validLoginUserLogin)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, validLoginUserLogin.UserProfileId.ToString())
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(int.Parse(_configuration["Jwt:ExpiredDays"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}