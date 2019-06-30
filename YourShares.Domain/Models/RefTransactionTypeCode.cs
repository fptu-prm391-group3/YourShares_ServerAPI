using System.Collections.Generic;

namespace YourShares.Domain.Models
{
    public partial class RefTransactionTypeCode
    {
        public const string TransactionIn = "IN";
        public const string TransactionOut = "OUT";
        
        public RefTransactionTypeCode()
        {
            Transaction = new HashSet<Transaction>();
        }
        public string TransactionTypeCode { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
