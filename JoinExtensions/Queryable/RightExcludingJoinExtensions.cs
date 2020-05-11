using System;
using System.Linq;
using System.Linq.Expressions;

namespace JoinExtensions.Queryable
{
    public static class RightExcludingJoinExtensions
    {
        /// <summary>
        /// For A -> B join, this method takes only B side, excluding common items with A.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="leftKey"></param>
        /// <param name="rightKey"></param>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public static IQueryable<JoinItem<TLeft, TRight>>
            RightExcludingJoinExt<TLeft, TRight, TKey>(this IQueryable<TLeft> left,
                IQueryable<TRight> right,
                Expression<Func<TLeft, TKey>> leftKey,
                Expression<Func<TRight, TKey>> rightKey)
            where TLeft : class where TRight : class
        {
            var result = right
//                .AsExpandable()
                .GroupJoin(left, rightKey, leftKey, (r, l) => new {r, l})
                .SelectMany(a => a.l.DefaultIfEmpty(), (a, r) => new {a, r})
                .Where(a => a.r == null)
                .Select(a => new JoinItem<TLeft, TRight>
                {
                    Left = null,
                    Right = a.a.r
                });
            return result;
        }
    }
}