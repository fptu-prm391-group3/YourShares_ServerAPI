using System;

namespace YourShares.Domain.Models
{
    public partial class Company
    {
        public Guid CompanyId { get; set; }
        public Guid AdminUserId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public long Capital { get; set; }
        public long TotalShare { get; set; }
        public long OptionPollAmount { get; set; }
    }
}
