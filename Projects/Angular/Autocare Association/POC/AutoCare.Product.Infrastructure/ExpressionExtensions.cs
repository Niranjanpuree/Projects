using System;
using System.Linq.Expressions;

namespace AutoCare.Product.Infrastructure
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> firstCondition,
            Expression<Func<T, bool>> conditionToBeIncluded)
        {
            if (conditionToBeIncluded == null)
            {
                return firstCondition;
            }

            if (firstCondition == null)
            {
                return conditionToBeIncluded;
            }

            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(firstCondition.Body, conditionToBeIncluded.Body),
                firstCondition.Parameters[0]);
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> firstCondition,
            Expression<Func<T, bool>> conditionToBeIncluded)
        {
            if (conditionToBeIncluded == null)
            {
                return firstCondition;
            }

            if (firstCondition == null)
            {
                return conditionToBeIncluded;
            }

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(firstCondition.Body, conditionToBeIncluded.Body),
                firstCondition.Parameters[0]);
        }
    }
}
