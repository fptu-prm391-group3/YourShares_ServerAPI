using System;

namespace YourShares.Domain.Models
{
    public partial class Transaction
    {
        public Guid Id { get; set; }
        public Guid ShareholderId { get; set; }
        public byte[] TimeStamp { get; set; }
        public int? Type { get; set; }
        public Guid SellerId { get; set; }
        public Guid BuyerId { get; set; }
        public long? ShareAmount { get; set; }
        public int? ShareType { get; set; }
        public long? Value { get; set; }
    }
}
