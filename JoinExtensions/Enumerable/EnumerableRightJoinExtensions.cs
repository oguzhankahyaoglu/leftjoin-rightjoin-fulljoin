using System;
using System.Collections.Generic;

namespace JoinExtensions.Enumerable
{
    public static class EnumerableRightJoinExtensions
    {
        /// <summary>
        /// DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database.
        /// In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD
        /// </summary>
        [Obsolete(
            "DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database. In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD")]
        public static IEnumerable<TResult> RightJoinExtEnumerable<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKey,
            Func<TRight, TKey> rightKey,
            Func<TLeft, TRight, TResult> resultFunc
        )
        {
            var query = EnumerableLeftJoinExtensions.LeftJoinExtEnumerable(right, left, rightKey, leftKey, (i, o) => resultFunc(o, i));
            return query;
        }
    }
}