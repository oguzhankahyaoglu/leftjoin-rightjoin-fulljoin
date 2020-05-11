using System;
using System.Linq;
using System.Linq.Expressions;

namespace JoinExtensions.Queryable
{
    public static class RightJoinExtensions
    {
        public static IQueryable<JoinItem<TLeft, TRight>> RightJoinExt<TLeft, TRight, TKey>(
            this IQueryable<TLeft> left,
            IQueryable<TRight> right,
            Expression<Func<TLeft, TKey>> leftKey,
            Expression<Func<TRight, TKey>> rightKey
        )
        {
            var query = LeftJoinExtensions.LeftJoinExt(right, left, rightKey, leftKey);
            return query.Select(t => new JoinItem<TLeft, TRight>
            {
                Left = t.Right,
                Right = t.Left
            });
        }
    }
}