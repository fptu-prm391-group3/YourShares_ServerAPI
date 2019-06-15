using System;

namespace YourShares.Domain.Models
{
    public partial class Transaction
    {
        public Guid TransactionId { get; set; }
        public Guid ShareAccountId { get; set; }
        public byte[] TransactionDate { get; set; }
        public long TransactionAmount { get; set; }
        public long TransactionValue { get; set; }
        public short TransactionStatus { get; set; }
        public short TransactionType { get; set; }
    }
}
