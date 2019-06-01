using System;
using System.Threading.Tasks;

namespace YourShares.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        YourSharesContext Context { get; }
        void Commit();
        Task CommitAsync();
    }
}