using System.ComponentModel.DataAnnotations;

namespace YourShares.Identity.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required] [EmailAddress] public string Email { get; set; }
    }
}