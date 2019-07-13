using System;
using System.Collections.Generic;
using System.Text;

namespace YourShares.Application.ViewModels
{
    public class RoundInvesterUpdateModel
    {
        public string InvestorName { get; set; }
        public long ShareAmount { get; set; }
        public long InvestedValue { get; set; }
        public double SharesHoldingPercentage { get; set; }
        public string PhotoUrl { get; set; }
    }
}
