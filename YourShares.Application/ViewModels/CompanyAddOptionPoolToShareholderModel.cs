using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YourShares.Application.ViewModels
{
    public class CompanyAddOptionPoolToShareholderModel
    {
        [Required]
        public long RestrictedAmount { get; set; }
        [Required]
        public float ConvertibleRatio { get; set; }
        [Required]
        public long ConvertibleTime { get; set; }
    }
}
