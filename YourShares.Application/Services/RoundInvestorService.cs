using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourShares.Application.Interfaces;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;

namespace YourShares.Application.Services
{
    public class RoundInvestorService : IRoundInvestorService
    {
        private readonly IRepository<RoundInvestor> _roundInvestorRepository;

        public RoundInvestorService(IRepository<RoundInvestor> roundInvestorRepository)
        {
            _roundInvestorRepository = roundInvestorRepository;
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
    }
}