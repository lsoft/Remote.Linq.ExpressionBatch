using System;
using System.Linq;

namespace RemoteLinq.ExpressionBatch.Executor.Material
{
    public class Materialized<T>
    {
        public IQueryable Queryable
        {
            get;
        }

        public System.Linq.Expressions.Expression Expression
        {
            get;
        }

        public Materialized(
            IQueryable queryable,
            System.Linq.Expressions.Expression expression
            )
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            Queryable = queryable;
            Expression = expression;
        }
    }
}