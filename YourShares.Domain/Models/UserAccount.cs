using System;

namespace YourShares.Domain.Models
{
    public partial class UserAccount
    {
        public Guid UserProfileId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordHashAlgorithm { get; set; }
        public Guid PasswordSalt { get; set; }
        public string UserAccountStatusCode { get; set; }

        public virtual RefUserAccountStatusCode UserAccountStatusCodeNavigation { get; set; }
    }
}
