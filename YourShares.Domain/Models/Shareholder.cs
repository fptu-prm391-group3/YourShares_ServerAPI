using System;

namespace YourShares.Domain.Models
{
    public partial class Shareholder
    {
        public Guid Id { get; set; }
        public Guid? CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Role { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
