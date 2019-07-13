using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourShares.Application.ViewModels;
using YourShares.Domain.Models;

namespace YourShares.Application.Interfaces
{
    public interface IRoundInvestorService
    {
        Task<RoundInvestor> GetById(Guid id);

        Task<List<RoundInvestor>> GetByRoundId(Guid id);

        Task<RoundInvestor> InsertRoundInvestor(RoundInvestorCreateModel model);

        Task<RoundInvestor> UpdateRoundInvestor(Guid id, RoundInvesterUpdateModel model);

        Task DeleteRoundInvestor(Guid id);
    }
}