using System;

namespace YourShares.Domain.Models
{
    public partial class Round
    {
        public Guid RoundId { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public long MoneyRaised { get; set; }
        public long ShareIncreased { get; set; }
        public long RoundDate { get; set; }
    }
}
