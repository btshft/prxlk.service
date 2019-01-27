using System.Linq.Expressions;

namespace Prxlk.Shared.Expressions
{
    public class ExpressionParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _source;
        private readonly ParameterExpression _replacement;

        public ExpressionParameterReplacer(ParameterExpression source, ParameterExpression replacement)
        {
            _source = source;
            _replacement = replacement;
        }

        /// <inheritdoc />
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(_source == node ? _replacement : node);
        }
    }
}