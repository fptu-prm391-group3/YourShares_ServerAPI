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
using YourShares.Application.Exceptions;
using YourShares.Application.Interfaces;
using YourShares.Application.ViewModels;
using YourShares.Domain.Models;
using YourShares.Domain.Util;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserProfileService _userProfileService;
        private readonly IUserGoogleAccountService _googleAccountService;
        private readonly IFacebookAccountService _facebookAccountService;

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProfileService"></param>
        /// <param name="configuration"></param>
        /// <param name="googleAccountService"></param>
        /// <param name="facebookAccountService"></param>
        public AuthenticationController(IUserProfileService userProfileService, IConfiguration configuration,
            IUserGoogleAccountService googleAccountService, IFacebookAccountService facebookAccountService)
        {
            _userProfileService = userProfileService;
            _configuration = configuration;
            _googleAccountService = googleAccountService;
            _facebookAccountService = facebookAccountService;
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
            var token = BuildToken(user.UserProfileId, user.Email);
            return new UserLoginTokenModel
            {
                UserId = user.UserProfileId.ToString(),
                Jwt = token
            };
        }

        #endregion

        #region Login with Open Auth

        /// <summary>
        /// Login with open authentication. If first login, redirect to create
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("oauth")]
        [HttpGet]
        public async Task<UserLoginTokenModel> LoginWithOpenAuth([FromQuery] string auth, [FromQuery] string id)
        {
            UserProfile user;
            switch (auth)
            {
                case "google":
                    var googleAccount = await _googleAccountService.GetByGoogleId(id);
                    if (googleAccount == null)
                    {
                        Response.StatusCode = (int) HttpStatusCode.Accepted;
                        return null;
                    }

                    Response.StatusCode = (int) HttpStatusCode.OK;
                    user = await _userProfileService.GetById(googleAccount.UserProfileId);
                    break;
                case "facebook":
                    var facebookAccount = await _facebookAccountService.GetByFacebookId(id);
                    if (facebookAccount == null)
                    {
                        Response.StatusCode = (int) HttpStatusCode.Accepted;
                        return null;
                    }

                    Response.StatusCode = (int) HttpStatusCode.OK;
                    user = await _userProfileService.GetById(facebookAccount.UserProfileId);
                    break;
                default:
                    Response.StatusCode = (int) HttpStatusCode.NoContent;
                    return null;
            }

            var token = BuildToken(user.UserProfileId, user.Email);
            Response.StatusCode = (int) HttpStatusCode.OK;
            return new UserLoginTokenModel
            {
                UserId = user.UserProfileId.ToString(),
                Jwt = token
            };
        }

        #endregion

        #region Create account with Open Auth

        /// <summary>
        /// Create user profile at first time open auth login
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("oauth")]
        [HttpPost]
        public async Task CreateOAuthAccount([FromQuery] string auth, OAuthCreateModel model)
        {
            switch (auth)
            {
                case "google":
                    await _userProfileService.CreateGoogleProfile(model);
                    Response.Redirect($"oauth?auth={auth}&id={model.AccountId}");
                    break;
                case "facebook":
                    await _userProfileService.CreateFacebookProfile(model);
                    Response.Redirect($"oauth?auth={auth}&id={model.AccountId}");
                    break;
                default:
                    Response.StatusCode = (int) HttpStatusCode.NoContent;
                    break;
            }
        }

        #endregion

        #region Build Token

        private string BuildToken(Guid userId, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email)
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