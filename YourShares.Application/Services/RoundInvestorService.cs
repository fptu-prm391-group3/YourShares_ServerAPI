using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourShares.Application.Interfaces;
using YourShares.Application.ViewModels;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;

namespace YourShares.Application.Services
{
    public class RoundInvestorService : IRoundInvestorService
    {
        private readonly IRepository<RoundInvestor> _roundInvestorRepository;
        private readonly IUnitOfWork _unitOfWork;

        #region Contructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundInvestorService"/> class.
        /// </summary>
        /// <param name="roundInvestorRepository">The round investor repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public RoundInvestorService(IRepository<RoundInvestor> roundInvestorRepository, IUnitOfWork unitOfWork)
        {
            _roundInvestorRepository = roundInvestorRepository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Delete        
        /// <summary>
        /// Deletes the round investor.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Round Invester id not found</exception>
        public async Task DeleteRoundInvestor(Guid id)
        {
            var result = _roundInvestorRepository.GetById(id);
            if (result == null) throw new Exception("Round Invester id not found");
            _roundInvestorRepository.Delete(result);
            await _unitOfWork.CommitAsync();
        }
        #endregion

        #region Get by id        
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<RoundInvestor> GetById(Guid id)
        {
            return _roundInvestorRepository.GetById(id);
        }
        #endregion

        #region Get By RoundId        
        /// <summary>
        /// Gets the by round identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<List<RoundInvestor>> GetByRoundId(Guid id)
        {
            return _roundInvestorRepository.GetManyAsNoTracking(x => x.RoundId == id)
                .Select(x => new RoundInvestor
                {
                    RoundId = x.RoundId,
                    RoundInvestorId = x.RoundInvestorId,
                    InvestorName = x.InvestorName,
                    InvestedValue = x.InvestedValue,
                    ShareAmount = x.ShareAmount,
                    SharesHoldingPercentage = x.SharesHoldingPercentage,
                    PhotoUrl = x.PhotoUrl
                }).ToList();
        }
        #endregion

        #region Insert Round Investor        
        /// <summary>
        /// Inserts the round investor.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<RoundInvestor> InsertRoundInvestor(RoundInvestorCreateModel model)
        {
            var roundInvestor = new RoundInvestor
            {
                RoundId = model.RoundId,
                InvestorName = model.InvestorName,
                InvestedValue = model.InvestedValue,
                ShareAmount = model.ShareAmount,
                SharesHoldingPercentage = model.SharesHoldingPercentage,
                PhotoUrl = model.PhotoUrl
            };
            var inserted = _roundInvestorRepository.Insert(roundInvestor).Entity;
            await _unitOfWork.CommitAsync();
            return inserted;
        }
        #endregion

        #region Update Round Investor        
        /// <summary>
        /// Updates the round investor.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Round Invester id not found</exception>
        public async Task<RoundInvestor> UpdateRoundInvestor(Guid id, RoundInvesterUpdateModel model)
        {
            var result = _roundInvestorRepository.GetById(id);
            if (result == null) throw new Exception("Round Invester id not found");
            result.InvestedValue = model.InvestedValue;
            result.InvestorName = model.InvestorName;
            result.PhotoUrl = model.PhotoUrl;
            result.ShareAmount = model.ShareAmount;
            result.SharesHoldingPercentage = model.SharesHoldingPercentage;
            _roundInvestorRepository.Update(result);
            await _unitOfWork.CommitAsync();
            return result;
        }
        #endregion
    }
}