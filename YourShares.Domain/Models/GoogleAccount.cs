using System;

namespace YourShares.Domain.Models
{
    public partial class GoogleAccount
    {
        public Guid UserProfileId { get; set; }
        public string GoogleAccountId { get; set; }
    }
}
