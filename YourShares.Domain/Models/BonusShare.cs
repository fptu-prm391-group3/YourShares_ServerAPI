using System;

namespace YourShares.Domain.Models
{
    public partial class BonusShare
    {
        public Guid BonusShareId { get; set; }
        public Guid ShareAccountId { get; set; }
        public byte[] AssignDate { get; set; }
        public long ConvertibleTime { get; set; }
        public double ConvertibleRatio { get; set; }
    }
}
