namespace YourShares.Domain.Models
{
    public partial class RefTransactionStatusCode
    {
        public const string Requested = "RQ";
        public const string Pending = "PD";
        public const string Accepted = "ACP";
        public const string Completed = "CMP";
        
        public string TransactionStatusCode { get; set; }
        public string Name { get; set; }
    }
}
