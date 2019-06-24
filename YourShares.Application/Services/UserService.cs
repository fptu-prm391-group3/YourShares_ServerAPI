using System;
using System.Linq;
using System.Threading.Tasks;
using YourShares.Application.Exceptions;
using YourShares.Application.Interfaces;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
using YourShares.Data.Interfaces;
using YourShares.Domain.Util;
using YourShares.RestApi.Models;

namespace YourShares.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserProfile> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork
            , IRepository<UserProfile> userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<UserViewDetailModel> GetById(Guid id)
        {
            var user = _userRepository.GetById(id);
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

        public async Task<IQueryable<UserSearchViewModel>> SearchUser(UserSearchModel model)
        {
            const string defaultSort = "Name ASC";
            var sortType = model.IsSortDesc ? "DESC" : "ASC";
            var sortField = ValidateUtils.IsNullOrEmpty(model.SortField)
                ? defaultSort
                : $"{model.SortField} {sortType}";
            var query = _userRepository.GetManyAsNoTracking(x =>
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
            return result;

        }

        public async Task<bool> UpdateEmail(UserEditEmailModel model)
        {
            var user = _userRepository.GetById(model.UserId);
            if (user == null) throw new EntityNotFoundException($"User id {model.UserId} not found");
            if (!ValidateUtils.IsMail(model.email))
            {
                throw new FormatException($"Email is wrong format");
            }
            user.Email = model.email;
            _userRepository.Update(user);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> UpdateInfo(UserEditInfoModel model)
        {
            var user = _userRepository.GetById(model.UserId);
            if (user == null) throw new EntityNotFoundException($"User id {model.UserId} not found");
            if (!ValidateUtils.IsNumber(model.Phone) || model.Phone.ToCharArray().Length != 10)
            {
                throw new FormatException($"{model.Phone} is wrong format");
            }
            if (ValidateUtils.IsNullOrEmpty(model.Address))
            {
                throw new FormatException($"Adresss not nullable");
            }
            user.Address = model.Address;
            user.Phone = model.Phone;
            _userRepository.Update(user);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
