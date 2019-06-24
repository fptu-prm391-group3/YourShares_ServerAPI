using System;

namespace YourShares.Application.ViewModels
{
    public class UserViewModel
    {
        public Guid UserProfileId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordHashAlgorithm { get; set; }
    }
}