namespace YourShares.Application.SearchModels
{
    public class CompanySearchModel : BaseSearchModel
    {
        public string AdminEmail { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public long Capital { get; set; }
    }
}