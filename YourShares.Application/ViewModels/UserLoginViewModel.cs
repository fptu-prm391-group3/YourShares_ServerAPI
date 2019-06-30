using System;

namespace YourShares.Application.ViewModels
{
    public class UserLoginViewModel
    {
        public Guid UserProfileId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordHashAlgorithm { get; set; }
        public Guid PasswordSalt { get; set; }
    }
}