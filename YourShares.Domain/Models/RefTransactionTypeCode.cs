namespace YourShares.Domain.Models
{
    public partial class RefTransactionTypeCode
    {
        public const string Debit = "DBT";
        public const string Credit = "CRD";
        
        public string TransactionTypeCode { get; set; }
        public string Name { get; set; }
    }
}
