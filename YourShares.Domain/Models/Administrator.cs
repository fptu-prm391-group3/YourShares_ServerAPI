using System;
using System.Collections.Generic;

namespace YourShares.Domain.Models
{
    public partial class Administrator
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
