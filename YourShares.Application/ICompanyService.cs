using System.Threading.Tasks;

namespace YourShares.Application
{
    public interface ICompanyService
    {
        Task<string> GetDetail();
    }
}