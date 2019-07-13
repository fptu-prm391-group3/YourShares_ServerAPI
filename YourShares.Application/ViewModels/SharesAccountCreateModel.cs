using System;
using System.Collections.Generic;
using System.Text;

namespace YourShares.Application.ViewModels
{
    public class SharesAccountCreateModel
    {
        public Guid ShareholderId { get; set; }
        public long ShareAmount { get; set; }
        public string ShareTypeCode { get; set; }
    }
}
