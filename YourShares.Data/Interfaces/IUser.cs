using System.Collections.Generic;
using System.Security.Claims;

namespace YourShares.Data.Interfaces
{
    public interface IUser
    {
        string Name { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
    }
}