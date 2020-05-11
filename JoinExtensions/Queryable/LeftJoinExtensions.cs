using System;
using System.Linq;
using System.Linq.Expressions;

namespace JoinExtensions.Queryable
{
    public static class LeftJoinExtensions
    {
        public static IQueryable<JoinItem<TLeft, TRight>> LeftJoinExt<TLeft, TRight, TKey>(
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
                    (outerItem, innerItems) => new {outerItem, innerItems})
                .SelectMany(
                    joinResult => joinResult.innerItems.DefaultIfEmpty(),
                    (joinResult, innerItem) =>
                        new JoinItem<TLeft, TRight>
                        {
                            Left = joinResult.outerItem,
                            Right = innerItem
                        });

            return result;
        }
    }
}