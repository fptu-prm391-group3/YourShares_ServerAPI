using System;

namespace YourShares.Application.ViewModels
{
    public class RoundInvestorCreateModel
    {
        public Guid RoundId { get; set; }
        public string InvestorName { get; set; }
        public long ShareAmount { get; set; }
        public long InvestedValue { get; set; }
        public double SharesHoldingPercentage { get; set; }
        public string PhotoUrl { get; set; }
    }
}