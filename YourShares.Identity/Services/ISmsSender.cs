using System.Threading.Tasks;

namespace YourShares.Identity.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}