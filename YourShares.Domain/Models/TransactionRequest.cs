using System;

namespace YourShares.Domain.Models
{
    public partial class TransactionRequest
    {
        public Guid TransactionRequestId { get; set; }
        public Guid TransactionInId { get; set; }
        public Guid TransactionOutId { get; set; }
        public Guid ApproverId { get; set; }
        public string RequestMessage { get; set; }
    }
}
