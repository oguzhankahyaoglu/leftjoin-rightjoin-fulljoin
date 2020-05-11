using System;
using System.Linq;
using System.Linq.Expressions;

namespace JoinExtensions.Queryable
{
    public static class FullOuterExcludingJoinExtensions
    {
        /// <summary>
        /// This method will take only A & B sides ,excluding A+B common items.
        /// </summary>
        public static IQueryable<JoinItem<TLeft, TRight>>
            FullOuterExcludingJoinExt<TLeft, TRight, TKey>(
                this IQueryable<TLeft> left,
                IQueryable<TRight> right,
                Expression<Func<TLeft, TKey>> leftKey,
                Expression<Func<TRight, TKey>> rightKey)
            where TLeft : class where TRight : class
        {
            var leftResult = LeftExcludingJoinExtensions.LeftExcludingJoinExt(left, right, leftKey, rightKey);
            var rightResult = RightExcludingJoinExtensions.RightExcludingJoinExt(left, right, leftKey, rightKey);
            return leftResult.Union(rightResult);
        }
    }
}