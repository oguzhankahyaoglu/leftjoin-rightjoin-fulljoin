using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace JoinExtensions
{
    public static class RightJoinExtensions
    {
        public static IQueryable<Tuple<TLeft, TRight>> RightJoin<TLeft, TRight, TKey>(
            this IQueryable<TLeft> left, 
            IQueryable<TRight> right,
            Expression<Func<TLeft, TKey>> leftKey,
            Expression<Func<TRight, TKey>> rightKey
        )
        {
            var query = right.LeftJoin(left, rightKey, leftKey);
            return query.Select(t=> Tuple.Create(t.Item2, t.Item1));
        }
        
        /// <summary>
        /// DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database.
        /// In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD
        /// </summary>
        [Obsolete("DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database. In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD")]
        public static IEnumerable<TResult> RightJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> left, 
            IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKey,
            Func<TRight, TKey> rightKey,
            Func<TLeft, TRight, TResult> resultFunc
        )
        {
            var query = right.LeftJoin(left, rightKey, leftKey, (i, o) => resultFunc(o, i));
            return query;
        }

    }
}
