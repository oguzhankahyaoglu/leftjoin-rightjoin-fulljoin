using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinExtensions.Enumerable
{
    // ReSharper disable PossibleMultipleEnumeration
    public static class EnumerableFullOuterJoinExtensions
    {
        /// <summary>
        /// Full outer join = Leftjoin + rightjoin
        /// </summary>
        public static IEnumerable<TResult> FullOuterJoinExtEnumerable<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> leftItems,
            IEnumerable<TRight> rightItems,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector,
            Func<TLeft, TRight, TResult> resultSelector)
        {
            var leftJoin =
                from left in leftItems
                join right in rightItems
                    on leftKeySelector(left) equals rightKeySelector(right) into temp
                from right in temp.DefaultIfEmpty()
                select resultSelector(left, right);

            var rightJoin =
                from right in rightItems
                join left in leftItems
                    on rightKeySelector(right) equals leftKeySelector(left) into temp
                from left in temp.DefaultIfEmpty()
                select resultSelector(left, right);

            return leftJoin.Union(rightJoin);
        }
    }
}