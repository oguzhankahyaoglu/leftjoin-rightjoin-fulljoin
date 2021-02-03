using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinExtensions.Enumerable
{
    public static class EnumerableLeftJoinExtensions
    {
        /// <summary>
        /// DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database.
        /// In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD
        /// </summary>
        public static IEnumerable<JoinItem<TLeft, TRight>> LeftJoinExtEnumerable<TLeft, TRight, TKey>(
            this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKey,
            Func<TRight, TKey> rightKey
        )
        {
            var query = LeftJoinExtEnumerable(left, right, leftKey, rightKey, (l, r) => new JoinItem<TLeft, TRight>
            {
                Left = l,
                Right = r
            });
            return query;
        }    
        
        /// <summary>
        /// DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database.
        /// In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD
        /// </summary>
        public static IEnumerable<TResult> LeftJoinExtEnumerable<TLeft, TRight, TKey, TResult>(
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
        
        /// <summary>
        /// LeftJoin.IEnumerable'ın joinCondition lambda ile çalışacak hali 
        /// </summary>
        public static IEnumerable<TResult> LeftJoinExtEnumerable<TLeft, TRight, TResult>(
            this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TRight, bool> joinCondition,
            Func<TLeft, TRight, TResult> resultFunc
        )
        {
            /*
var query2 = (
    from users in Repo.T_User
    from mappings in Repo.T_User_Group
         .Where(mapping => mapping.USRGRP_USR == users.USR_ID)
         .DefaultIfEmpty() // <== makes join left join
             */
            var query =
                from l in left
                from r in right.Where(rr => joinCondition(l, rr))
                    .DefaultIfEmpty()
                select resultFunc(l, r);
            return query;
        }
    }
}