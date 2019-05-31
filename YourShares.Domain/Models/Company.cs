using System;

namespace YourShares.Domain.Models
{
    public class Company
    {
        // Empty constructor for EF
        public Guid Id { get; set; }

        public string CompanyCode { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }
    }
}