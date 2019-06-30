using System.Collections.Generic;

namespace YourShares.Domain.Models
{
    public partial class RefUserAccountStatusCode
    {
        public const string Guest = "GST";
        public const string VerifiedUser = "USR";
        
        public RefUserAccountStatusCode()
        {
            UserAccount = new HashSet<UserAccount>();
        }

        public string UserAccountStatusCode { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserAccount> UserAccount { get; set; }

    }
}
