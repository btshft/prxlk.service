using System;
using System.Linq.Expressions;
using Prxlk.Shared.Expressions;

namespace Prxlk.Domain.Specifications.Shared
{
    public static class SpecificationExtensions
    {
        public static ISpecification<TEntity> And<TEntity>(this ISpecification<TEntity> source, ISpecification<TEntity> and) 
            where TEntity : class
        {
            var leftExpression = source.AsExpression();
            var rightExpression = and.AsExpression();
            var replaceVisitor = new ExpressionParameterReplacer(
                rightExpression.Parameters[0],
                leftExpression.Parameters[0]);

            var andExpression = Expression.Lambda<Func<TEntity, bool>>(
                Expression.AndAlso(
                    leftExpression.Body,
                    replaceVisitor.Visit(rightExpression.Body)), leftExpression.Parameters[0]);
            
            return new Specification<TEntity>(andExpression);
        }

        public static ISpecification<TEntity> AndNot<TEntity>(this ISpecification<TEntity> source,
            ISpecification<TEntity> and)
            where TEntity : class
        {
            var andSpecExpression = source.And(and).AsExpression();
            
            return new Specification<TEntity>(
                Expression.Lambda<Func<TEntity, bool>>(
                    Expression.Not(andSpecExpression.Body), 
                    andSpecExpression.Parameters));
        }

        public static ISpecification<TEntity> Or<TEntity>(this ISpecification<TEntity> source, ISpecification<TEntity> or) 
            where TEntity : class
        {
            var leftExpression = source.AsExpression();
            var rightExpression = or.AsExpression();
            var replaceVisitor = new ExpressionParameterReplacer(
                rightExpression.Parameters[0],
                leftExpression.Parameters[0]);
            
            var orExpression = Expression.Lambda<Func<TEntity, bool>>(
                Expression.OrElse(
                    leftExpression.Body,
                    replaceVisitor.Visit(rightExpression.Body)), leftExpression.Parameters[0]);
            
            return new Specification<TEntity>(orExpression);
        }

        public static ISpecification<TEntity> OrNot<TEntity>(this ISpecification<TEntity> source,
            ISpecification<TEntity> or)
            where TEntity : class
        {
            var orExpression = source.Or(or).AsExpression();
            
            return new Specification<TEntity>(
                Expression.Lambda<Func<TEntity, bool>>(
                    Expression.Not(orExpression.Body), orExpression.Parameters));
        }
    }
}