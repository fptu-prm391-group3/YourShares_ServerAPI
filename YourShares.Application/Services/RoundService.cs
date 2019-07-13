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
    public class RoundService : IRoundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Round> _roundRepository;

        #region Contructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundService"/> class.
        /// </summary>
        /// <param name="roundRepository">The round repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public RoundService(IRepository<Round> roundRepository, IUnitOfWork unitOfWork)
        {
            _roundRepository = roundRepository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Get By Id        
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Round> GetById(Guid id)
        {
            return _roundRepository.GetById(id);
        }
        #endregion

        #region Get By Company Id        
        /// <summary>
        /// Gets the by company identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<List<Round>> GetByCompanyId(Guid id)
        {
            return _roundRepository.GetManyAsNoTracking(x => x.CompanyId == id).OrderBy(x => x.RoundDate)
                .Select(x => new Round
                {
                    RoundId = x.RoundId,
                    CompanyId = x.CompanyId,
                    Name = x.Name,
                    MoneyRaised = x.MoneyRaised,
                    ShareIncreased = x.ShareIncreased,
                    RoundDate = x.RoundDate
                }).ToList();
        }
        #endregion

        #region Insert Round        
        /// <summary>
        /// Inserts the round.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<Round> InsertRound(RoundCreateModel model)
        {
            var round = new Round
            {
                Name = model.Name,
                CompanyId = model.CompanyId,
                MoneyRaised = model.PreRoundShares,
                ShareIncreased = model.PostRoundShares,
                RoundDate = model.TimestampRound,
            };
            var inserted = _roundRepository.Insert(round).Entity;
            await _unitOfWork.CommitAsync();
            return inserted;
        }
        #endregion

        #region update        
        /// <summary>
        /// Updates the round.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Round id not found</exception>
        public async Task<Round> UpdateRound(Guid id, RoundUpdateModel model)
        {
            var result = _roundRepository.GetById(id);
            if (result == null) throw new Exception("Round id not found");
            result.MoneyRaised = model.MoneyRaised;
            result.Name = model.Name;
            result.RoundDate = model.RoundDate;
            result.ShareIncreased = model.ShareIncreased;
            _roundRepository.Update(result);
            await _unitOfWork.CommitAsync();
            return result;
        }
        #endregion

        #region Delete        
        /// <summary>
        /// Deletes the round.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Round id not found</exception>
        public async Task DeleteRound(Guid id)
        {
            var result = _roundRepository.GetById(id);
            if (result == null) throw new Exception("Round id not found");
            _roundRepository.Delete(result);
            await _unitOfWork.CommitAsync();
        }
        #endregion
    }
}