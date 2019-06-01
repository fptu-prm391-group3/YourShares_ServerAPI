using YourShares.Application.ViewModels;

namespace YourShares.Application.SearchModels
{
    public class CompanySearchModel : BaseSearchModel
    {
        public string AdminUserName { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Capital { get; set; }
    }
}