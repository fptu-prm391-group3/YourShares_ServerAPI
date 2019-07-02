using System.ComponentModel.DataAnnotations;

namespace YourShares.Application.ViewModels
{
    public class CompanyCreateModel
    {
        [Required]
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        [Required]
        public long Capital { get; set; }
        [Required]
        public long TotalShares { get; set; }
    }
}