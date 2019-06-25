using System;
using System.Collections.Generic;
using System.Text;

namespace YourShares.Application.ViewModels
{
    public class UserEditInfoModel
    {
        public Guid UserId { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
    }
}
