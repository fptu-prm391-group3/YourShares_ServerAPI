using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using YourShares.Application.Interfaces;
using YourShares.Application.ViewModels;
using YourShares.Domain.Util;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IConfiguration _configuration;

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProfileService"></param>
        /// <param name="configuration"></param>
        public AuthenticationController(IUserProfileService userProfileService, IConfiguration configuration)
        {
            _userProfileService = userProfileService;
            _configuration = configuration;
        }
        #endregion

        #region Login
        /// <summary>
        /// Login using registered account with basic authentication
        /// </summary>
        /// <returns>Authorized bearer token</returns>
        [Route("auth")]
        [HttpGet]
        public async Task<UserLoginTokenModel> LoginWithEmail()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return null;
            }
            var auth = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]).Parameter;
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(auth)).Split(":");
            return await AuthenticateUser(credentials[0], credentials[1]);
        }
        
        private async Task<UserLoginTokenModel> AuthenticateUser(string email, string password)
        {
            var user = await _userProfileService.GetUserByEmail(email);
            if (HashingUtils.HashString(password + user.PasswordSalt, user.PasswordHashAlgorithm) != user.PasswordHash)
            {
                Response.StatusCode = (int) HttpStatusCode.NotFound;
                return null;
            }

            Response.StatusCode = (int) HttpStatusCode.OK;
            var token = BuildToken(user);
            return new UserLoginTokenModel
            {
                UserId = user.UserProfileId.ToString(),
                Jwt = token
            };
        }
        #endregion

        [Route("oauth")]
        [HttpGet]
        public async Task LoginWithOpenAuth([FromQuery] string auth)
        {
            switch (auth)
            {
                case "google":
                    break;
                case "facebook":
                    break;
            }
            
        } 

        #region Build Token

        private string BuildToken(UserLoginViewModel validLoginUserLogin)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, validLoginUserLogin.UserProfileId.ToString()),
                new Claim(ClaimTypes.Email, validLoginUserLogin.Email)
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(int.Parse(_configuration["Jwt:ExpiredDays"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion

        #region Register

        /// <summary>
        /// Register a user account in system.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("auth")]
        [HttpPost]
        public async Task Register([FromBody] UserCreateViewModel model)
        {
            await _userProfileService.CreateUserProfile(new UserRegisterModel
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
            Response.StatusCode = (int) HttpStatusCode.Created;
        }
        #endregion
        
    }
}