using System;

namespace YourShares.Domain.Models
{
    public partial class Transaction
    {
        public Guid TransactionId { get; set; }
        public Guid ShareAccountId { get; set; }
        public long TransactionAmount { get; set; }
        public byte[] TransactionDate { get; set; }
        public string TransactionTypeCode { get; set; }
        public long TransactionValue { get; set; }
        public string TransactionStatusCode { get; set; }
    }
}
