using System;
using System.Collections.Generic;
using System.Text;

namespace YourShares.Application.ViewModels
{
    public class RoundUpdateModel
    {
        public string Name { get; set; }
        public long MoneyRaised { get; set; }
        public long ShareIncreased { get; set; }
        public long RoundDate { get; set; }
    }
}
