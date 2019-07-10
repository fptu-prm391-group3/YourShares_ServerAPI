using System;
using System.Collections.Generic;
using System.Text;

namespace YourShares.Application.ViewModels
{
    public class TransactionRequestModel
    {
        public string Message { get; set; }
        public Guid ReceiverProfileId { get; set; }
        public Guid ShareAccountId { get; set; }
        public Guid CompanyId { get; set; }
        public long Value { get; set; }
        public long Amount { get; set; }
    }
}
