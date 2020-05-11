using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinExtensions.Enumerable
{
    public static class EnumerableInnerJoinExtensions
    {
        /// <summary>
        /// DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database.
        /// In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD
        /// </summary>
        [Obsolete(
            "DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database. In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD")]
        public static IEnumerable<JoinItem<TLeft, TRight>> InnerJoinExt<TLeft, TRight, TKey>(
            this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKey,
            Func<TRight, TKey> rightKey
        )
        {
            var result = EnumerableLeftJoinExtensions.LeftJoinExtEnumerable(left, right, leftKey, rightKey)
                    .Where(a => a.Right != null)
                ;
            return result;
        }

        /// <summary>
        /// DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database.
        /// In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD
        /// </summary>
        [Obsolete(
            "DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database. In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD")]
        public static IEnumerable<TResult> InnerJoinExt<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKey,
            Func<TRight, TKey> rightKey,
            Func<TLeft, TRight, TResult> resultFunc
        )
        {
            var result = EnumerableLeftJoinExtensions.LeftJoinExtEnumerable(left, right, leftKey, rightKey)
                    .Where(a => a.Right != null)
                    .Select(a=> resultFunc(a.Left, a.Right))
                ;
            return result;
        }
    }
}