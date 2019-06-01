namespace YourShares.Application.SearchModels
{
    public class CompanySearchModel
    {
        public string AdminUserName { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string Capital { get; set; }
        public bool IsSortDesc { get; set; }
        public string SortField { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}