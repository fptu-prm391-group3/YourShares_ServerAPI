using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourShares.Application.ViewModels;
using YourShares.Domain.Models;

namespace YourShares.Application.Interfaces
{
    public interface IRoundService
    {
        Task<Round> GetById(Guid id);

        Task<List<Round>> GetByCompanyId(Guid id);

        Task<Round> InsertRound(RoundCreateModel model);

        Task<Round> UpdateRound(Guid id, RoundUpdateModel model);

        Task DeleteRound(Guid id);
    }
}