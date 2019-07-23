using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourShares.Application.Exceptions;
using YourShares.Application.Interfaces;
using YourShares.Application.SearchModels;
using YourShares.Application.ViewModels;
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
        private readonly IFacebookAccountService _facebookAccountService;
        private readonly IUserGoogleAccountService _googleAccountService;
        private readonly IUnitOfWork _unitOfWork;

        #region Contructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userProfileRepository">The user profile repository.</param>
        /// <param name="userAccountRepository">The user account repository.</param>
        /// <param name="userAccountService">The user account service.</param>
        /// <param name="googleAccountService">The google account service.</param>
        /// <param name="facebookAccountService">The facebook account service.</param>
        public UserProfileService(IUnitOfWork unitOfWork
            , IRepository<UserProfile> userProfileRepository,
            IRepository<UserAccount> userAccountRepository, IUserAccountService userAccountService,
            IUserGoogleAccountService googleAccountService, IFacebookAccountService facebookAccountService)
        {
            _unitOfWork = unitOfWork;
            _userProfileRepository = userProfileRepository;
            _userAccountRepository = userAccountRepository;
            _userAccountService = userAccountService;
            _googleAccountService = googleAccountService;
            _facebookAccountService = facebookAccountService;
        }
        #endregion

        #region        
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">User id {id} not found</exception>
        public async Task<UserProfile> GetById(Guid id)
        {
            var user = _userProfileRepository.GetById(id);
            if (user == null) throw new EntityNotFoundException($"User id {id} not found");
            return user;
        }
        #endregion

        #region Get User By Email        
        /// <summary>
        /// Gets the user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">
        /// User Profile not found
        /// or
        /// User Account not found. Try query in Google account
        /// </exception>
        public async Task<UserAccount> GetUserByEmail(string email)
        {
            var profile = _userProfileRepository.GetManyAsNoTracking(x => email.Equals(x.Email)).FirstOrDefault();
            if (profile == null) throw new EntityNotFoundException("User Profile not found");
            var result = _userAccountRepository.GetManyAsNoTracking(y => y.UserProfileId.Equals(profile.UserProfileId))
                .FirstOrDefault();
            if (result == null)
                throw new EntityNotFoundException("User Account not found. Try query in Google account");
            return new UserAccount
            {
                UserProfileId = result.UserProfileId,
                Email = result.Email,
                PasswordHash = result.PasswordHash,
                PasswordHashAlgorithm = result.PasswordHashAlgorithm,
                PasswordSalt = result.PasswordSalt
            };
        }
        #endregion

        #region Create        
        /// <summary>
        /// Creates the user profile.
        /// </summary>
        /// <param name="profileModel">The profile model.</param>
        /// <param name="accountModel">The account model.</param>
        /// <returns></returns>
        /// <exception cref="FormatException">
        /// Email address invalid
        /// or
        /// Phone number invalid
        /// or
        /// Email existed
        /// </exception>
        public async Task<bool> CreateUserProfile(UserRegisterModel profileModel
            , UserAccountCreateModel accountModel)
        {
            if (!ValidateUtils.IsMail(profileModel.Email)) throw new FormatException("Email address invalid");

            var query = _userProfileRepository.GetManyAsNoTracking(x => x.Email.Equals(profileModel.Email));
            if (query.ToList().Count != 0) throw new FormatException("Email existed");
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
        #endregion

        #region Create by GG        
        /// <summary>
        /// Creates the google profile.
        /// </summary>
        /// <param name="profileModel">The profile model.</param>
        /// <returns></returns>
        public async Task<bool> CreateGoogleProfile(OAuthCreateModel profileModel)
        {
            var googleAccount = await _googleAccountService.GetByGoogleId(profileModel.AccountId);
             if (googleAccount != null)
             {
                 return false;
             }
            var userProfile = new UserProfile
            {
                Email = profileModel.Email,
                FirstName = profileModel.FirstName,
                LastName = profileModel.LastName,
                PhotoUrl = profileModel.PhotoUrl
            };
            var inserted = _userProfileRepository.Insert(userProfile).Entity;
            return await _googleAccountService.CreateGoogleAccount(inserted.UserProfileId, profileModel.AccountId);
        }
        #endregion

        #region Create by FB        
        /// <summary>
        /// Creates the facebook profile.
        /// </summary>
        /// <param name="profileModel">The profile model.</param>
        /// <returns></returns>
        public async Task<bool> CreateFacebookProfile(OAuthCreateModel profileModel)
        {
            var facebookAccount =  await _facebookAccountService.GetByFacebookId(profileModel.AccountId);
            if (facebookAccount != null)
            {
                return false;
            }
            var userProfile = new UserProfile
            {
                Email = profileModel.Email,
                FirstName = profileModel.FirstName,
                LastName = profileModel.LastName,
                PhotoUrl = profileModel.PhotoUrl
            };
            var inserted = _userProfileRepository.Insert(userProfile).Entity;
            return await _facebookAccountService.CreateFacebookAccount(inserted.UserProfileId, profileModel.AccountId);
        }
        #endregion

        #region Search        
        /// <summary>
        /// Searches the user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
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
        #endregion

        #region Update Email        
        /// <summary>
        /// Updates the email.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">User id {id} not found</exception>
        /// <exception cref="FormatException">Email is wrong format</exception>
        public async Task<UserProfile> UpdateEmail(Guid id, string email)
        {
            var user = _userProfileRepository.GetById(id);
            if (user == null) throw new EntityNotFoundException($"User id {id} not found");
            if (!ValidateUtils.IsMail(email))
            {
                throw new FormatException($"Email is wrong format");
            }

            user.Email = email;
            _userProfileRepository.Update(user);
            await _unitOfWork.CommitAsync();
            return user;
        }
        #endregion

        #region Update LastName        
        /// <summary>
        /// Updates the last name.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">User id {id} not found</exception>
        /// <exception cref="FormatException">lastName not nullable</exception>
        public async Task<UserProfile> UpdateLastName(Guid id, string lastName)
        {
            var user = _userProfileRepository.GetById(id);
            if (user == null) throw new EntityNotFoundException($"User id {id} not found");
            if (ValidateUtils.IsNullOrEmpty(lastName))
            {
                throw new FormatException($"lastName not nullable");
            }

            user.LastName = lastName;
            _userProfileRepository.Update(user);
            await _unitOfWork.CommitAsync();
            return user;
        }
        #endregion

        #region Update FirstName        
        /// <summary>
        /// Updates the first name.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">User id {id} not found</exception>
        /// <exception cref="FormatException">firstName not nullable</exception>
        public async Task<UserProfile> UpdateFirstName(Guid id, string firstName)
        {
            var user = _userProfileRepository.GetById(id);
            if (user == null) throw new EntityNotFoundException($"User id {id} not found");
            if (ValidateUtils.IsNullOrEmpty(firstName))
            {
                throw new FormatException($"firstName not nullable");
            }

            user.FirstName = firstName;
            _userProfileRepository.Update(user);
            await _unitOfWork.CommitAsync();
            return user;
        }
        #endregion

        #region Update Address        
        /// <summary>
        /// Updates the address.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">User id {id} not found</exception>
        /// <exception cref="FormatException">Address not nullable</exception>
        public async Task<UserProfile> UpdateAddress(Guid id, string address)
        {
            var user = _userProfileRepository.GetById(id);
            if (user == null) throw new EntityNotFoundException($"User id {id} not found");
            if (ValidateUtils.IsNullOrEmpty(address))
            {
                throw new FormatException($"Address not nullable");
            }

            user.Address = address;
            _userProfileRepository.Update(user);
            await _unitOfWork.CommitAsync();
            return user;
        }
        #endregion

        #region Update Phone
        /// <summary>
        /// Updates the phone.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="phone">The phone.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">User id {id} not found</exception>
        /// <exception cref="FormatException"></exception>
        public async Task<UserProfile> UpdatePhone(Guid id, string phone)
        {
            var user = _userProfileRepository.GetById(id);
            if (user == null) throw new EntityNotFoundException($"User id {id} not found");
            if (!ValidateUtils.IsNumber(phone) || phone.ToCharArray().Length != 10)
            {
                throw new FormatException($"{phone} is wrong format");
            }

            user.Phone = phone;
            _userProfileRepository.Update(user);
            await _unitOfWork.CommitAsync();
            return user;
        }
        #endregion

        #region Update Info        
        /// <summary>
        /// Updates the information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">User id {model.UserId} not found</exception>
        /// <exception cref="FormatException">
        /// Address not nullable
        /// </exception>
        public async Task<bool> UpdateInfo(UserEditInfoModel model)
        {
            // TODO handle update email in user account, if google or facebook account don't allow update
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

            if (model.Address != null) user.Address = model.Address;
            if (model.Phone != null) user.Phone = model.Phone;
            _userProfileRepository.Update(user);
            await _unitOfWork.CommitAsync();
            return true;
        }
        #endregion

        #region Delete        
        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">User id {Id} not found</exception>
        public async Task DeleteUser(Guid Id)
        {
            var user = _userProfileRepository.GetById(Id);
            if (user == null) throw new EntityNotFoundException($"User id {Id} not found");
            _userProfileRepository.Delete(user);
            await _unitOfWork.CommitAsync();
        }
        #endregion
    }
}