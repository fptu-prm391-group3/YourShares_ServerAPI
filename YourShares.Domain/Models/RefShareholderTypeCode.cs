using System.Collections.Generic;

namespace YourShares.Domain.Models
{
    public partial class RefShareholderTypeCode
    {
        public const string Founders = "FD";
        public const string Shareholders = "SH";
        public const string Employees = "EMP";
        
        public RefShareholderTypeCode()
        {
            Shareholder = new HashSet<Shareholder>();
        }

        public string ShareholderTypeCode { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Shareholder> Shareholder { get; set; }
    }
}
