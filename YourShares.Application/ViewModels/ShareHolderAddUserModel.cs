using System;
using System.Collections.Generic;
using System.Text;

namespace YourShares.Application.ViewModels
{
    public class ShareHolderAddUserModel
    {
        public Guid CompanyId { get; set; }

        public Guid UserId { get; set; }

        public string ShareholderType { get; set; }
    }
}
