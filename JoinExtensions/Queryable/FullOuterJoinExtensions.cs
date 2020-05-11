using System;
using System.Linq;
using System.Linq.Expressions;

namespace JoinExtensions.Queryable
{
    // ReSharper disable PossibleMultipleEnumeration
    public static class FullOuterJoinExtensions
    {
        public static IQueryable<JoinItem<TLeft, TRight>> FullOuterJoinJoinExt<TLeft, TRight, TKey>(
            this IQueryable<TLeft> left,
            IQueryable<TRight> right,
            Expression<Func<TLeft, TKey>> leftKey,
            Expression<Func<TRight, TKey>> rightKey)
            where TLeft : class where TRight : class
        {
            var leftResult = LeftJoinExtensions.LeftJoinExt(left, right, leftKey, rightKey);
            var rightResult = RightJoinExtensions.RightJoinExt(left, right, leftKey, rightKey);
            return leftResult.Union(rightResult);
        }
    }
}