using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;

namespace JoinExtensions
{
    public static class LeftExcludingJoinExtensions
    {
        /// <summary>
        /// For A -> B join, this method takes only A side, excluding common items with B.
        /// </summary>
        /// <param name="leftSide"></param>
        /// <param name="rightSide"></param>
        /// <param name="leftKey"></param>
        /// <param name="rightKey"></param>
        /// <param name="resultFunc">RIGHT WILL ALWAYS RETURN NULL</param>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IQueryable<TResult> LeftExcludingJoin<TLeft, TRight, TKey, TResult>(
            this IQueryable<TLeft> leftSide,
                IQueryable<TRight> rightSide,
                Expression<Func<TLeft, TKey>> leftKey,
                Expression<Func<TRight, TKey>> rightKey,
                Func<TLeft, TRight,TResult> resultFunc)
            where TLeft : class where TRight : class
        {
            var result = leftSide
                .AsExpandable()
                .GroupJoin(rightSide, leftKey, rightKey, (l, r) => new {l, r})
                .SelectMany(a => a.r.DefaultIfEmpty(), (a, r) => new {a, r})
                .Where(a => a.r == null)
                .Select(a => resultFunc(a.a.l, a.r));
            return result;
        } 
        
        /// <summary>
        /// For A -> B join, this method takes only A side, excluding common items with B.
        /// DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database.
        /// In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD
        /// </summary>
        /// <param name="leftSide"></param>
        /// <param name="rightSide"></param>
        /// <param name="leftKey"></param>
        /// <param name="rightKey"></param>
        /// <param name="resultFunc">RIGHT WILL ALWAYS RETURN NULL</param>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        [Obsolete("DO NOT USE THIS OVERLOAD (Ienumerable) with EntityFramework or Database-related logic, since it will directly enumerate the query to database. In order to ensure that your query works on your database, USE IQUERYABLE OVERLOAD")]
        public static IEnumerable<TResult> LeftExcludingJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> leftSide,
                IEnumerable<TRight> rightSide,
                Func<TLeft, TKey> leftKey,
                Func<TRight, TKey> rightKey,
                Func<TLeft, TRight, TResult> resultFunc)
            where TLeft : class where TRight : class
        {
            var _result = leftSide
                .GroupJoin(rightSide, leftKey, rightKey, (l, r) => new {l, r})
                .SelectMany(a => a.r.DefaultIfEmpty(), (a, r) => new {a, r})
                .Where(a => a.r == null)
                .Select(a => resultFunc(a.a.l, null));

            return _result;
        } 

    }
}