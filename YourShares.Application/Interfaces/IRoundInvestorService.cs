using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourShares.Domain.Models;

namespace YourShares.Application.Interfaces
{
    public interface IRoundInvestorService
    {
        Task<RoundInvestor> GetById(Guid id);

        Task<List<RoundInvestor>> GetByRoundId(Guid id);
    }
}