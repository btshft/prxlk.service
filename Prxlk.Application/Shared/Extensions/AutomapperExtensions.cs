using System;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Prxlk.Application.Shared.Extensions
{
    public static class AutomapperExtensions
    {
        public static Expression<Func<TIn, TOut>> GetProjection<TIn, TOut>(this IMapper mapper)
        {
            return mapper.ConfigurationProvider.ExpressionBuilder.GetMapExpression<TIn, TOut>();
        }
    }
}