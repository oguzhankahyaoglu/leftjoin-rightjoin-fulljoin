using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinExtensions.Enumerable
{
    public static class EnumerableRightExcludingJoinExtensions
    {
        /// <summary>
        /// For A -> B join, this method takes only B side, excluding common items with A.
        /// DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database.
        /// In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="leftKey"></param>
        /// <param name="rightKey"></param>
        /// <param name="resultFunc">LEFT WILL ALWAYS RETURN NULL</param>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        [Obsolete(
            "DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database. In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD")]
        public static IEnumerable<TResult>
            RightExcludingJoinExtEnumerable<TLeft, TRight, TKey, TResult>(this IEnumerable<TLeft> left,
                IEnumerable<TRight> right,
                Func<TLeft, TKey> leftKey,
                Func<TRight, TKey> rightKey,
                Func<TLeft, TRight, TResult> resultFunc)
            where TLeft : class where TRight : class
        {
            var result = right
                .GroupJoin(left, rightKey, leftKey, (r, l) => new {r, l})
                .SelectMany(a => a.l.DefaultIfEmpty(), (a, r) => new {a, r})
                .Where(a => a.r == null)
                .Select(a => resultFunc(null, a.a.r));
            return result;
        }
    }
}