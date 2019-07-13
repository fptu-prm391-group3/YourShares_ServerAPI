using System;

namespace YourShares.Domain.Models
{
    public partial class Transaction
    {
        public Guid TransactionId { get; set; }
        public Guid ShareAccountId { get; set; }
        public long TransactionAmount { get; set; }
        public long TransactionDate { get; set; }
        public string TransactionTypeCode { get; set; }
        public long TransactionValue { get; set; }
        public string TransactionStatusCode { get; set; }
        public string Message { get; set; }

        public virtual RefTransactionStatusCode TransactionStatusCodeNavigation { get; set; }
        public virtual RefTransactionTypeCode TransactionTypeCodeNavigation { get; set; }
    }
}
