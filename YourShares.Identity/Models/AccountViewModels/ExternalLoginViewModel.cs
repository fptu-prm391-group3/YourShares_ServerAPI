using System.ComponentModel.DataAnnotations;

namespace YourShares.Identity.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required] [EmailAddress] public string Email { get; set; }
    }
}