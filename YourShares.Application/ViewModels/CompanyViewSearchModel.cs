using System;
using System.Collections.Generic;
using System.Text;

namespace YourShares.Application.ViewModels
{
    public class CompanyViewSearchModel
    {
        public Guid CompanyId { get; set; }
        public Guid AdminProfileId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public long Capital { get; set; }
        public long TotalShares { get; set; }
        public long OptionPollAmount { get; set; }

        public string AdminName { get; set; }
    }
}
