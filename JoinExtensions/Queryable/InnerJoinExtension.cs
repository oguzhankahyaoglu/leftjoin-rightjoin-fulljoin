using System;
using System.Linq;
using System.Linq.Expressions;

namespace JoinExtensions.Queryable
{
    public static class InnerJoinExtensions
    {
        public static IQueryable<JoinItem<TLeft, TRight>> InnerJoinExt<TLeft, TRight, TKey>(
            this IQueryable<TLeft> left,
            IQueryable<TRight> right,
            Expression<Func<TLeft, TKey>> leftKey,
            Expression<Func<TRight, TKey>> rightKey
        )
        {
            var result = LeftJoinExtensions.LeftJoinExt(left, right, leftKey, rightKey)
                .Where(a => a.Right != null);
            return result;
        }
    }
}