using System;

namespace YourShares.Application.ViewModels
{
    public class OAuthCreateModel
    {
        public string AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
    }
}