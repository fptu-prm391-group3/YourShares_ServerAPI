namespace YourShares.Domain.Models
{
    public partial class RefShareTypeCode
    {
        public const string Preference = "PRF01";
        public const string Standard = "STD02";
        public const string Restricted = "RST03";
        
        public string ShareTypeCode { get; set; }
        public string Name { get; set; }
    }
}
