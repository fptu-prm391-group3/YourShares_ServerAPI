using System;
using System.Collections.Generic;
using System.Text;

namespace YourShares.Application.ViewModels
{
    public class SharesAccountViewModel
    {
        public Guid ShareAccountId { get; set; }
        public long ShareAmount { get; set; }
        public string Name { get; set; }
        public float ShareAmountRatio { get; set; }
        public float RatioConvert { get; set; }
        public DateTime? TimeConvert { get; set; }
    }
}
