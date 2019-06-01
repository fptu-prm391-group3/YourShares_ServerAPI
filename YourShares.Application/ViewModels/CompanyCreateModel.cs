namespace YourShares.Application.ViewModels
{
    public class CompanyCreateModel
    {
        public string CompanyName { get; set; }

        public string Address { get; set; }
        
        public string Phone { get; set; }
        public long Capital { get; set; }

        public long TotalShares { get; set; }

        public long OptionPoll { get; set; }
    }
}