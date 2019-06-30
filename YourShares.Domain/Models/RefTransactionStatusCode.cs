using System.Collections.Generic;

namespace YourShares.Domain.Models
{
    public partial class RefTransactionStatusCode
    {
        public const string Requested = "RQ";
        public const string Pending = "PD";
        public const string Accepted = "ACP";
        public const string Completed = "CMP";
        
        public RefTransactionStatusCode()
        {
            Transaction = new HashSet<Transaction>();
        }

        public string TransactionStatusCode { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
