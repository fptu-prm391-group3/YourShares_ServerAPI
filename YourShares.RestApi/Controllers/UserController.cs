﻿using System;
 using System.Diagnostics;
 using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YourShares.Application.Interfaces;
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
        [Route("{id}")]
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
        [Route("/api/user/email")]
        public async Task UpdateInfo([FromBody] UserEditEmailModel model)
        {
            await _userProfileService.UpdateEmail(model);
            Response.StatusCode = (int)HttpStatusCode.OK;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task Register([FromBody]UserCreateModel model) 
        {
            
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("auth")]
        public async Task<string> LoginWithEmail([FromBody]UserCreateModel model)
        {
            Trace.WriteLine($"Logging in as {model.Email} and {model.Password}");
            var users = await _userProfileService.SearchUserByEmail(model.Email, 1);
            UserViewModel user;
            if (users.Count > 0)
            {
                user = users[0];
            }
            else
            {
                Response.StatusCode = (int) HttpStatusCode.NotFound;
                return "NOTFOUND";
            }
            if (model.Password == user.PasswordHash)
            {
                var token = BuildToken(user);
                Console.WriteLine(user.Email);
                Console.WriteLine(user.PasswordHash);
                return $"{{\"jwt\":\"{token}\"}}";
            }

            Response.StatusCode = (int) HttpStatusCode.BadRequest;
            return "Wrong pass";
        }
        
        private string BuildToken(UserViewModel validLoginUser)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, validLoginUser.UserProfileId.ToString())
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