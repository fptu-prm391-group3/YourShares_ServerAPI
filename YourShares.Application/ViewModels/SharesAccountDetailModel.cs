using System;
using System.Collections.Generic;
using System.Text;

namespace YourShares.Application.ViewModels
{
    public class SharesAccountDetailModel
    {
        public Guid ShareAccountId { get; set; }
        public Guid ShareholderId { get; set; }
        public long ShareAmount { get; set; }
        public string Name { get; set; }
    }
}
