using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace JoinExtensions
{
    // ReSharper disable PossibleMultipleEnumeration
    public static class FullOuterJoinExtensions
    {
        public static IQueryable<JoinItem<TLeft, TRight>> FullOuterJoinJoin<TLeft, TRight, TKey>(
            this IQueryable<TLeft> left,
            IQueryable<TRight> right,
            Expression<Func<TLeft, TKey>> leftKey,
            Expression<Func<TRight, TKey>> rightKey)
            where TLeft : class where TRight : class
        {
            var leftResult = left.LeftJoin(right, leftKey, rightKey);
            var rightResult = left.RightJoin(right, leftKey, rightKey);
            return leftResult.Union(rightResult);
        }
        
        /// <summary>
        /// DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database.
        /// In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD
        /// </summary>
        [Obsolete("DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database. In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD")]
        public static IEnumerable<TResult> FullOuterJoinJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKey,
            Func<TRight, TKey> rightKey,
            Func<TLeft, TRight, TResult> result)
            where TLeft : class where TRight : class
        {
            var leftResult = left.LeftJoin(right, leftKey, rightKey, result);
            var rightResult = left.RightJoin(right, leftKey, rightKey, result);
            return leftResult.Union(rightResult);
        }

    }
}