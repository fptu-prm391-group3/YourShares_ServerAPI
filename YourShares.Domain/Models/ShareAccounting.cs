using System;
using System.Collections.Generic;

namespace YourShares.Domain.Models
{
    public partial class ShareAccounting
    {
        public Guid Id { get; set; }
        public Guid SharehoderId { get; set; }
        public int? Type { get; set; }
        public long? Amount { get; set; }
    }
}
