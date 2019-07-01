using System;

namespace YourShares.Application.ViewModels
{
    public class CompanyUpdateModel
    {
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public long Capital { get; set; }
        public long TotalShares { get; set; }
        public long OptionPoll { get; set; }
    }
}