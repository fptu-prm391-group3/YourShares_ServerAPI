using System.Threading.Tasks;
using YourShares.Data.Interfaces;

namespace YourShares.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(Context context)
        {
            Context = context;
        }

        public Context Context { get; }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public async Task CommitAsyn()
        {
            await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}