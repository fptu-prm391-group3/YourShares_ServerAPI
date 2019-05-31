using System;
using System.Threading.Tasks;

namespace YourShares.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Context Context { get; }
        void Commit();
        Task CommitAsyn();
    }
}