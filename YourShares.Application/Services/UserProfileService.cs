using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using YourShares.Application.Exceptions;
using YourShares.Application.Interfaces;
using YourShares.Application.ViewModels;
using YourShares.Data.Interfaces;
using YourShares.Domain.Util;
using YourShares.RestApi.Models;

namespace YourShares.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IRepository<UserAccount> _userAccountRepository;

        public UserProfileService(IUnitOfWork unitOfWork
            , IRepository<UserProfile> userProfileRepository, IRepository<UserAccount> userAccountRepository)
        {
            _unitOfWork = unitOfWork;
            _userProfileRepository = userProfileRepository;
            _userAccountRepository = userAccountRepository;
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

        public async Task<List<UserViewModel>> SearchUserByEmail(string email, int maxResult)
        {
            var query = _userProfileRepository.GetManyAsNoTracking(x =>
                    ValidateUtils.IsNullOrEmpty(email) || x.Email.ToUpper().Contains(email.ToUpper()))
                .Select(field => new
                {
                    field.UserProfileId,
                    field.FirstName,
                    field.LastName,
                    field.Email
                }).Join(_userAccountRepository.GetManyAsNoTracking(x => ValidateUtils.IsNullOrEmpty(email)),
                    x => x.UserProfileId, y => y.UserProfileId, (x, y) =>
                        new UserViewModel
                        {
                            UserProfileId = x.UserProfileId,
                            Email = x.Email,
                            PasswordHash = y.PasswordHash,
                            PasswordHashAlgorithm = y.PasswordHashAlgorithm,
                        });
            var result = query.Take(maxResult).ToList();
            return result;
        }

        public Task<UserProfile> CreateUserProfile(UserCreateModel model)
        {
            throw new NotImplementedException();
        }
    }
}