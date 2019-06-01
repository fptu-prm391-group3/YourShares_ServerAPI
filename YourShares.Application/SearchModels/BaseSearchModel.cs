namespace YourShares.Application.SearchModels
{
    public class BaseSearchModel
    {
        public bool IsSortDesc { get; set; } = false;

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string SortField { get; set; }
    }
}