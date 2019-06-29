using System;

namespace YourShares.Domain.Models
{
    public partial class FacebookAccount
    {
        public Guid UserProfileId { get; set; }
        public string FacebookAccountId { get; set; }
    }
}
