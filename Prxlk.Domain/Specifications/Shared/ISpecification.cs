using System;
using System.Linq.Expressions;

namespace Prxlk.Domain.Specifications.Shared
{
    public interface ISpecification<TEntity> where TEntity : class
    {
        bool IsSatisfiedBy(TEntity entity);
        Expression<Func<TEntity, bool>> AsExpression();
    }
}