using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinExtensions.Enumerable
{
    // ReSharper disable PossibleMultipleEnumeration
    public static class EnumerableFullOuterJoinExtensions
    {
        /// <summary>
        /// DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database.
        /// In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD
        /// </summary>
        [Obsolete(
            "DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database. In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD")]
        public static IEnumerable<TResult> FullOuterJoinExtEnumerable<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKey,
            Func<TRight, TKey> rightKey,
            Func<TLeft, TRight, TResult> result)
            where TLeft : class where TRight : class
        {
            var leftResult = EnumerableLeftJoinExtensions.LeftJoinExtEnumerable(left, right, leftKey, rightKey, result);
            var rightResult = EnumerableRightJoinExtensions.RightJoinExtEnumerable(left, right, leftKey, rightKey, result);
            return leftResult.Union(rightResult);
        }
    }
}