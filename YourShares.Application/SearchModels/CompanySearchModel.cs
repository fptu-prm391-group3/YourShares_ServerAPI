using YourShares.Application.ViewModels;

namespace YourShares.Application.SearchModels
{
    public class CompanySearchModel : BaseSearchModel
    {
        public string AdminUserName { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public long Capital { get; set; }
    }
}