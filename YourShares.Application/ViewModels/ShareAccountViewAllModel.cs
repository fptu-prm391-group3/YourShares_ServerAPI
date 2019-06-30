using System;
using System.Collections.Generic;
using System.Text;

namespace YourShares.Application.ViewModels
{
    public class ShareAccountViewAllModel
    {
        public string Type { get; set; }

        public string UserName { get; set; }

        public List<SharesAccountViewModel> ListAccount { get; set; }
    }
}
