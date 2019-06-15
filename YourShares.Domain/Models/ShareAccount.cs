using System;

namespace YourShares.Domain.Models
{
    public partial class ShareAccount
    {
        public Guid ShareAccountId { get; set; }
        public Guid ShareholderId { get; set; }
        public long ShareAmount { get; set; }
        public short? ShareType { get; set; }
    }
}
