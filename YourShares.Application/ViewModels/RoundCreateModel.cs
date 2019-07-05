using System;

namespace YourShares.Application.ViewModels
{
    public class RoundCreateModel
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public long PreRoundShares { get; set; }
        public long PostRoundShares { get; set; }
        public long TimestampRound { get; set; }
    }
}