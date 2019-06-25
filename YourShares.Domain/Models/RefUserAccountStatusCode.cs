namespace YourShares.Domain.Models
{
    public partial class RefUserAccountStatusCode
    {
        public const string GUEST = "GST";
        public const string VERIFIED_USER = "USR";
        
        public string UserAccountStatusCode { get; set; }
        public string Name { get; set; }

    }
}
