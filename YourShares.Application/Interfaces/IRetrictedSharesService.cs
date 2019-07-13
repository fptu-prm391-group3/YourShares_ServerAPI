using System;
using System.Threading.Tasks;

namespace YourShares.Application.Interfaces
{
    public interface IRetrictedSharesService
    {
        Task AddRetrictedShares(float ConvertibleRatio, long ConvertibleTime,Guid ShareAccountId);
    }
}
