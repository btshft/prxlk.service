using System.Data;

namespace Prxlk.Domain.DataAccess
{
    public interface IDataSessionFactory
    {
        IDataSession CreateSession();
        IDataSession CreateSession(IsolationLevel isolationLevel);
    }
}