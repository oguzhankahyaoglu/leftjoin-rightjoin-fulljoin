using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace JoinExtensions
{
    public static class LeftJoinExtensions
    {
        public static IQueryable<Tuple<TLeft, TRight>> LeftJoin<TLeft, TRight, TKey>(
            this IQueryable<TLeft> left, 
            IQueryable<TRight> right,
            Expression<Func<TLeft, TKey>> leftKey,
            Expression<Func<TRight, TKey>> rightKey
            )
        {
            /*
            var query = (
                    from l in left
                    join r in right on leftKey(l) equals rightKey(r)
                    into j1
                    from r1 in j1.DefaultIfEmpty()
                    select resultFunc(l, r1)
                    );
             */
            var result = left
//                        .AsExpandable()// Tell LinqKit to convert everything into an expression tree.
                        .GroupJoin(
                            right,
                            leftKey,
                            rightKey,
                            (outerItem, innerItems) => new { outerItem, innerItems })
                        .SelectMany(
                            joinResult => joinResult.innerItems.DefaultIfEmpty(),
                            (joinResult, innerItem) => 
                                Tuple.Create(joinResult.outerItem, innerItem));
        
            return result;
        }     
        
        /// <summary>
        /// DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database.
        /// In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD
        /// </summary>
        [Obsolete("DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database. In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD")]
        public static IEnumerable<TResult> LeftJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> left, 
            IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKey,
            Func<TRight, TKey> rightKey,
            Func<TLeft, TRight, TResult> resultFunc
            )
        {
            /*
            var query = (
                    from l in left
                    join r in right on leftKey(l) equals rightKey(r)
                    into j1
                    from r1 in j1.DefaultIfEmpty()
                    select resultFunc(l, r1)
                    );
             */
            var query = left
                .GroupJoin(right, leftKey, rightKey, (l, j1) => new {l, j1})
                .SelectMany(t => t.j1.DefaultIfEmpty(), (t, r1) => resultFunc(t.l, r1));
            return query;
        }
    }
}
