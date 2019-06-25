namespace YourShares.Domain.Models
{
    public partial class RefShareholderTypeCode
    {
        public const string Founders = "FD";
        public const string Shareholders = "SH";
        public const string Employees = "EMP";
        
        public string ShareholderTypeCode { get; set; }
        public string Name { get; set; }
    }
}
