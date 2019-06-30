using System;
using System.Collections.Generic;
using System.Text;

namespace YourShares.Application.ViewModels
{
   public class ShareholderDetailModel
    {
        public Guid ShareholderId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid UserProfileId { get; set; }
        public string ShareholderType { get; set; }
    }
}
