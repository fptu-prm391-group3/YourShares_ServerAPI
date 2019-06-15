using System;

namespace YourShares.Application.ViewModels
{
    public class CompanyViewModel
    {
        public Guid Id { get; set; }
        public Guid AdminUserId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public long Capital { get; set; }
        public long TotalShare { get; set; }
        public long? OptionPoll { get; set; }
    }
}