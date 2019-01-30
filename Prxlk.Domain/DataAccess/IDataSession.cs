using System;
using System.Threading;
using System.Threading.Tasks;

namespace Prxlk.Domain.DataAccess
{   
    public interface IDataSession : IDisposable
    {
        Task CommitAsync(CancellationToken cancellation);
        Task RollbackAsync(CancellationToken cancellation);
    }
}