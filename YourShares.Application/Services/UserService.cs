using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YourShares.Application.Exceptions;
using YourShares.Application.Interfaces;
using YourShares.Application.ViewModels;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;
using YourShares.Domain.Util;

namespace YourShares.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork
            , IRepository<User> userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
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
