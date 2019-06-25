using System;
using System.Collections.Generic;

namespace YourShares.RestApi.Models
{
    public partial class UserAccount
    {
        public Guid UserProfileId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordHashAlgorithm { get; set; }
        public string PasswordReminderToken { get; set; }
        public string EmailConfirmationToken { get; set; }
        public string UserAccountStatusCode { get; set; }
    }
}
