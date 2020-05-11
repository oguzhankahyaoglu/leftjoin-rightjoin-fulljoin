using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinExtensions.Enumerable
{
    public static class EnumerableFullOuterExcludingJoinExtensions
    {
        /// <summary>
        /// This method will take only A & B sides ,excluding A+B common items.
        /// DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database.
        /// In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD
        /// </summary>
        [Obsolete(
            "DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database. In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD")]
        public static IEnumerable<TResult>
            FullOuterExcludingJoinExtEnumerable<TLeft, TRight, TKey, TResult>(
                this IEnumerable<TLeft> left,
                IEnumerable<TRight> right,
                Func<TLeft, TKey> leftKey,
                Func<TRight, TKey> rightKey,
                Func<TLeft, TRight, TResult> resultFunc)
            where TLeft : class where TRight : class
        {
            var l = left.ToArray();
            var r = right.ToArray();
            var leftResult = EnumerableLeftExcludingJoinExtensions.LeftExcludingJoinExtEnumerable(l, r, leftKey, rightKey, resultFunc);
            var rightResult = EnumerableRightExcludingJoinExtensions.RightExcludingJoinExtEnumerable(l, r, leftKey, rightKey, resultFunc);
            return leftResult.Union(rightResult);
        }
    }
}