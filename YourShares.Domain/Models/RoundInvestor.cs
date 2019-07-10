using System;

namespace YourShares.Domain.Models
{
    public partial class RoundInvestor
    {
        public Guid RoundInvestorId { get; set; }
        public Guid RoundId { get; set; }
        public string InvestorName { get; set; }
        public long ShareAmount { get; set; }
        public long InvestedValue { get; set; }
        public double SharesHoldingPercentage { get; set; }
        public string PhotoUrl { get; set; }
    }
}
