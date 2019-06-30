using System;

namespace YourShares.Domain.Models
{
    public partial class Round
    {
        public Guid RoundId { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public long PreRoundShares { get; set; }
        public long PostRoundShares { get; set; }
    }
}
