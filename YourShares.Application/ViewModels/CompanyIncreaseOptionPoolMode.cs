using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YourShares.Application.ViewModels
{
    public class CompanyIncreaseOptionPoolMode
    {
        [Required]
        public Guid CompanyId { get; set; }
        [Required]
        public long sharesAmount { get; set; }
    }
}
