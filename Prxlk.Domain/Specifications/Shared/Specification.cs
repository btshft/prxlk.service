using System;
using System.Linq.Expressions;

namespace Prxlk.Domain.Specifications.Shared
{
    public class Specification<TEntity> : ISpecification<TEntity> where TEntity : class
    {
        public static Specification<TEntity> True = new Specification<TEntity>(_ => true);
        public static Specification<TEntity> False = new Specification<TEntity>(_ => false);
        
        private readonly Lazy<Func<TEntity, bool>> _compiledPredicateProvider;

        protected Func<TEntity, bool> PredicateCompiled => _compiledPredicateProvider.Value;
        protected Expression<Func<TEntity, bool>> PredicateExpression { get; set; }

        protected Specification()
        {
            _compiledPredicateProvider = new Lazy<Func<TEntity, bool>>(() =>
            {
                if (PredicateExpression == null)
                    throw new InvalidOperationException($"{nameof(PredicateExpression)} is null");

                return PredicateExpression.Compile();
            });
        }
        
        public Specification(Expression<Func<TEntity, bool>> expression) 
            : this()
        {
            PredicateExpression = expression;
        }
        
        /// <inheritdoc />
        public bool IsSatisfiedBy(TEntity entity)
        {
            return PredicateCompiled(entity);
        }

        /// <inheritdoc />
        public Expression<Func<TEntity, bool>> AsExpression()
        {
            return PredicateExpression;
        }
    }
}