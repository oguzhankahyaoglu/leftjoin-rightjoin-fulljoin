using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace JoinExtensions
{
    public static class FullOuterExcludingJoinExtensions
    {
        /// <summary>
        /// This method will take only A & B sides ,excluding A+B common items.
        /// </summary>
        public static IQueryable<JoinItem<TLeft, TRight>>
            FullOuterExcludingJoin<TLeft, TRight, TKey>(
                this IQueryable<TLeft> left,
                IQueryable<TRight> right,
                Expression<Func<TLeft, TKey>> leftKey,
                Expression<Func<TRight, TKey>> rightKey)
            where TLeft : class where TRight : class
        {
            var leftResult = left.LeftExcludingJoin(right, leftKey, rightKey);
            var rightResult = left.RightExcludingJoin(right, leftKey, rightKey);
            return leftResult.Union(rightResult);
        }

        /// <summary>
        /// This method will take only A & B sides ,excluding A+B common items.
        /// DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database.
        /// In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD
        /// </summary>
        [Obsolete(
            "DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database. In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD")]
        public static IEnumerable<TResult>
            FullOuterExcludingJoin<TLeft, TRight, TKey, TResult>(
                this IEnumerable<TLeft> left,
                IEnumerable<TRight> right,
                Func<TLeft, TKey> leftKey,
                Func<TRight, TKey> rightKey,
                Func<TLeft, TRight, TResult> resultFunc)
            where TLeft : class where TRight : class
        {
            var l = left.ToArray();
            var r = right.ToArray();
            var leftResult = l.LeftExcludingJoin(r, leftKey, rightKey, resultFunc);
            var rightResult = l.RightExcludingJoin(r, leftKey, rightKey, resultFunc);
            return leftResult.Union(rightResult);
        }
    }
}