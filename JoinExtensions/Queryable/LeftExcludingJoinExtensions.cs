using System;
using System.Linq;
using System.Linq.Expressions;

namespace JoinExtensions.Queryable
{
    public static class LeftExcludingJoinExtensions
    {
        /// <summary>
        /// For A -> B join, this method takes only A side, excluding common items with B.
        /// </summary>
        /// <param name="leftSide"></param>
        /// <param name="rightSide"></param>
        /// <param name="leftKey"></param>
        /// <param name="rightKey"></param>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public static IQueryable<JoinItem<TLeft, TRight>> LeftExcludingJoinExt<TLeft, TRight, TKey>(
            this IQueryable<TLeft> leftSide,
            IQueryable<TRight> rightSide,
            Expression<Func<TLeft, TKey>> leftKey,
            Expression<Func<TRight, TKey>> rightKey)
            where TLeft : class where TRight : class
        {
            var result = leftSide
//                .AsExpandable()
                .GroupJoin(rightSide, leftKey, rightKey, (l, r) => new {l, r})
                .SelectMany(a => a.r.DefaultIfEmpty(), (a, r) => new {a, r})
                .Where(a => a.r == null)
                .Select(a => new JoinItem<TLeft, TRight>
                {
                    Left = a.a.l,
                    Right = a.r
                });
            return result;
        }
    }
}