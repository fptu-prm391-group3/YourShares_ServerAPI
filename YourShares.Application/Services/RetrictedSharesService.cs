using System;
using System.Threading.Tasks;
using YourShares.Application.Interfaces;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;

namespace YourShares.Application.Services
{
    public class RetrictedSharesService : IRetrictedSharesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<RestrictedShare> _restrictedShareRepository;

        #region Contructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="RetrictedSharesService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="restrictedShareRepository">The restricted share repository.</param>
        public RetrictedSharesService(IUnitOfWork unitOfWork
            , IRepository<RestrictedShare> restrictedShareRepository)
        {
            _unitOfWork = unitOfWork;
            _restrictedShareRepository = restrictedShareRepository;
        }
        #endregion

        #region Add Retricted Shares        
        /// <summary>
        /// Adds the retricted shares.
        /// </summary>
        /// <param name="ConvertibleRatio">The convertible ratio.</param>
        /// <param name="ConvertibleTime">The convertible time.</param>
        /// <param name="ShareAccountId">The share account identifier.</param>
        /// <returns></returns>
        public async Task AddRetrictedShares(float ConvertibleRatio, long ConvertibleTime, Guid ShareAccountId)
        {
            var query = _restrictedShareRepository.GetById(ShareAccountId);
            if (query == null)
            {
                _restrictedShareRepository.Insert(new RestrictedShare
                {
                    ConvertibleTime = ConvertibleTime,
                    AssignDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    ConvertibleRatio = ConvertibleRatio,
                    ShareAccountId = ShareAccountId
                });
            }
            else
            {
                query.ConvertibleRatio = ConvertibleRatio;
                _restrictedShareRepository.Update(query);
            }
            
            await _unitOfWork.CommitAsync();
        }
        #endregion
    }
}
