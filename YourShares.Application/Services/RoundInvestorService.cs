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

        public RoundInvestorService(IRepository<RoundInvestor> roundInvestorRepository, IUnitOfWork unitOfWork)
        {
            _roundInvestorRepository = roundInvestorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RoundInvestor> GetById(Guid id)
        {
            return _roundInvestorRepository.GetById(id);
        }

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
                    SharesHoldingPercentage = x.SharesHoldingPercentage
                }).ToList();
        }

        public async Task<RoundInvestor> InsertRoundInvestor(RoundInvestorCreateModel model)
        {
            var inserted = _roundInvestorRepository.Insert(new RoundInvestor
            {
                RoundId = model.RoundId,
                InvestorName = model.InvestorName,
                InvestedValue = model.InvestedValue,
                ShareAmount = model.ShareAmount,
                SharesHoldingPercentage = model.SharesHoldingPercentage
            }).Entity;
            await _unitOfWork.CommitAsync();
            return inserted;
        }
    }
}