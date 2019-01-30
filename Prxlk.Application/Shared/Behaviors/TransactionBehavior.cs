using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Prxlk.Application.Shared.Messages;
using Prxlk.Domain.DataAccess;

namespace Prxlk.Application.Shared.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IDataSessionFactory _sessionFactory;

        public TransactionBehavior(IDataSessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!(request is Command)) 
                return await next();
            
            using (var session = _sessionFactory.CreateSession())
            {
                try
                {
                    var result = await next();
                    await session.CommitAsync(cancellationToken);
                    return result;
                }
                catch (Exception e)
                {
                    await session.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}