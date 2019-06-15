using System;

namespace YourShares.Domain.Models
{
    public partial class Shareholder
    {
        public Guid ShareholderId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public short ShareholderRole { get; set; }
    }
}
