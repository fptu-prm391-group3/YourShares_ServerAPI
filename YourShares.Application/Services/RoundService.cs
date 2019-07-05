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

        public RoundService(IRepository<Round> roundRepository, IUnitOfWork unitOfWork)
        {
            _roundRepository = roundRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Round> GetById(Guid id)
        {
            return _roundRepository.GetById(id);
        }

        public async Task<List<Round>> GetByCompanyId(Guid id)
        {
            return _roundRepository.GetManyAsNoTracking(x => x.CompanyId == id).OrderBy(x => x.RoundDate)
                .Select(x => new Round
                {
                    RoundId = x.RoundId,
                    CompanyId = x.CompanyId,
                    Name = x.Name,
                    PreRoundShares = x.PreRoundShares,
                    PostRoundShares = x.PostRoundShares,
                    RoundDate = x.RoundDate
                }).ToList();
        }

        public async Task<Round> InsertRound(RoundCreateModel model)
        {
            var round = new Round
            {
                Name = model.Name,
                CompanyId = model.CompanyId,
                PreRoundShares = model.PreRoundShares,
                PostRoundShares = model.PostRoundShares,
                RoundDate=model.TimestampRound,
            };
            var inserted = _roundRepository.Insert(round).Entity;
            await _unitOfWork.CommitAsync();
            return inserted;
        }
    }
}