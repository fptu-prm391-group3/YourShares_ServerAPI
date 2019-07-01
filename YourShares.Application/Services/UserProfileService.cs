using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using YourShares.Application.Exceptions;
using YourShares.Application.Interfaces;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
using YourShares.Data;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;
using YourShares.Domain.Util;

namespace YourShares.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IRepository<UserAccount> _userAccountRepository;
        private readonly IUserAccountService _userAccountService;
        private readonly IUnitOfWork _unitOfWork;

        public UserProfileService(IUnitOfWork unitOfWork
            , IRepository<UserProfile> userProfileRepository,
            IRepository<UserAccount> userAccountRepository, IUserAccountService userAccountService)
        {
            _unitOfWork = unitOfWork;
            _userProfileRepository = userProfileRepository;
            _userAccountRepository = userAccountRepository;
            _userAccountService = userAccountService;
        }

        public async Task<UserViewDetailModel> GetById(Guid id)
        {
            var user = _userProfileRepository.GetById(id);
            if (user == null) throw new EntityNotFoundException($"User id {id} not found");

            return new UserViewDetailModel
            {
                UserId = user.UserProfileId,
                Address = user.Address,
                Email = user.Email,
                Name = $"{user.FirstName} {user.LastName}",
                Phone = user.Phone
            };
        }

        public async Task<UserAccount> GetUserByEmail(string email)
        {
            var profile = _userProfileRepository.GetManyAsNoTracking(x => email.Equals(x.Email)).FirstOrDefault();
            if (profile == null) throw new EntityNotFoundException("User Profile not found");
            var result = _userAccountRepository.GetManyAsNoTracking(y => y.UserProfileId.Equals(profile.UserProfileId))
                .FirstOrDefault();
            if (result == null) throw new EntityNotFoundException("User Account not found. Try query in Google account");
            return new UserAccount
            {
                UserProfileId = result.UserProfileId,
                Email = result.Email,
                PasswordHash = result.PasswordHash,
                PasswordHashAlgorithm = result.PasswordHashAlgorithm,
                PasswordSalt = result.PasswordSalt

            };
        }

        public async Task<bool> CreateUserProfile(UserRegisterModel profileModel
            , UserAccountCreateModel accountModel)
        {
            if (!ValidateUtils.IsMail(profileModel.Email)) throw new FormatException("Email address invalid");
            if (!ValidateUtils.IsPhone(profileModel.Phone)) throw new FormatException("Phone number invalid");

            var query = _userProfileRepository.GetManyAsNoTracking(x => x.Email.Equals(profileModel.Email));
            if(query.ToList().Count!=0) throw new FormatException("Email Exited");
            var userProfile = _userProfileRepository.Insert(new UserProfile
            {
                Email = profileModel.Email,
                FirstName = profileModel.FirstName,
                LastName = profileModel.LastName,
                Phone = profileModel.Phone,
                Address = profileModel.Address
            });
            return await _userAccountService.CreateUserAccount(accountModel, userProfile.Entity.UserProfileId);
        }

        public Task<bool> CreateGoogleProfile(UserRegisterModel profileModel, string googleAccountId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateFacebookAccountProfile(UserRegisterModel profileModel, string facebookAccountId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserSearchViewModel>> SearchUser(UserSearchModel model)
        {
            const string defaultSort = "Name ASC";
            var sortType = model.IsSortDesc ? "DESC" : "ASC";
            var sortField = ValidateUtils.IsNullOrEmpty(model.SortField)
                ? defaultSort
                : $"{model.SortField} {sortType}";
            var query = _userProfileRepository.GetManyAsNoTracking(x =>
                    ValidateUtils.IsNullOrEmpty(model.Name)
                    || x.FirstName.ToUpper().Contains(model.Name.ToUpper())
                    && ValidateUtils.IsNullOrEmpty(model.Name)
                    || x.LastName.ToUpper().Contains(model.Name.ToUpper())
                    && ValidateUtils.IsNullOrEmpty(model.Phone)
                    || x.Phone.Equals(model.Name)
                    && ValidateUtils.IsNullOrEmpty(model.Email)
                    || x.Email.ToUpper().Contains(model.Email.ToUpper()))
                .Select(x => new UserSearchViewModel
                {
                    Address = x.Address,
                    UserId = x.UserProfileId,
                    Email = x.Email,
                    Name = $"{x.FirstName} {x.LastName}"
                });
            var result = query.Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize);
            return result.ToList();
        }

        public async Task<bool> UpdateEmail(UserEditEmailModel model)
        {
            var user = _userProfileRepository.GetById(model.UserId);
            if (user == null) throw new EntityNotFoundException($"User id {model.UserId} not found");
            if (!ValidateUtils.IsMail(model.email))
            {
                throw new FormatException($"Email is wrong format");
            }

            user.Email = model.email;
            _userProfileRepository.Update(user);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> UpdateInfo(UserEditInfoModel model)
        {
            var user = _userProfileRepository.GetById(model.UserId);
            if (user == null) throw new EntityNotFoundException($"User id {model.UserId} not found");
            if (!ValidateUtils.IsNumber(model.Phone) || model.Phone.ToCharArray().Length != 10)
            {
                throw new FormatException($"{model.Phone} is wrong format");
            }

            if (ValidateUtils.IsNullOrEmpty(model.Address))
            {
                throw new FormatException($"Address not nullable");
            }

            user.Address = model.Address;
            user.Phone = model.Phone;
            _userProfileRepository.Update(user);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}