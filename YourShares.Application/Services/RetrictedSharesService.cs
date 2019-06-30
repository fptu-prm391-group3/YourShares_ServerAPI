using System;
using System.Collections.Generic;
using System.Text;
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

        public RetrictedSharesService(IUnitOfWork unitOfWork
            , IRepository<RestrictedShare> restrictedShareRepository)
        {
            _unitOfWork = unitOfWork;
            _restrictedShareRepository = restrictedShareRepository;
        }
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
    }
}
