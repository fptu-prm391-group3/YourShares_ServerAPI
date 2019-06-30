using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourShares.Application.Interfaces;
using YourShares.Data.Interfaces;
using YourShares.Domain.Models;

namespace YourShares.Application.Services
{
    public class RoundService :IRoundService
    {
        private readonly IRepository<Round> _roundRepository;

        public RoundService(IRepository<Round> roundRepository)
        {
            _roundRepository = roundRepository;
        }

        public async Task<Round> GetById(Guid id)
        {
            return _roundRepository.GetById(id);
        }

        public async Task<List<Round>> GetByCompanyId(Guid id)
        {
            return _roundRepository.GetManyAsNoTracking(x => x.CompanyId == id)
                .Select(x => new Round
                {
                    RoundId = x.RoundId,
                    CompanyId = x.CompanyId,
                    Name = x.Name,
                    PreRoundShares = x.PreRoundShares,
                    PostRoundShares = x.PostRoundShares
                }).ToList();
        }
    }
}