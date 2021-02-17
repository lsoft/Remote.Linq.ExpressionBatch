using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RemoteLinq.ExpressionBatch.Executor.Material
{
    public static class Materializer
    {
        private static readonly MethodInfo _fod;
        private static readonly MethodInfo _f;
        //private static readonly MethodInfo _die;

        static Materializer()
        {
            _fod = typeof(Enumerable)
                    .GetMethods()
                    .First(m => m.Name == nameof(System.Linq.Enumerable.FirstOrDefault) && m.GetParameters().Length == 1)
                ;
            _f = typeof(Enumerable)
                    .GetMethods()
                    .First(m => m.Name == nameof(System.Linq.Enumerable.First) && m.GetParameters().Length == 1)
                ;
            //_die = typeof(Enumerable)
            //        .GetMethods()
            //        .First(m => m.Name == nameof(System.Linq.Enumerable.DefaultIfEmpty) && m.GetParameters().Length == 1)
            //    ;
        }

        public static Materialized<T> AsFirstOrDefault<T>(
            this IQueryable<T> q
            )
        {
            if (q == null)
            {
                throw new ArgumentNullException(nameof(q));
            }

            return new Materialized<T>(
                q,
                ModifyWith(q, _fod)
                );
        }

        public static Materialized<T> AsFirst<T>(
            this IQueryable<T> q
            )
        {
            if (q == null)
            {
                throw new ArgumentNullException(nameof(q));
            }

            return new Materialized<T>(
                q,
                ModifyWith(q, _f)
                );
        }

        public static Materialized<IReadOnlyList<T>> AsList<T>(
            this IQueryable<T> q
            )
        {
            if (q == null)
            {
                throw new ArgumentNullException(nameof(q));
            }

            return new Materialized<IReadOnlyList<T>>(
                q,
                q.Expression
                );
        }


        private static System.Linq.Expressions.Expression ModifyWith<T>(
            IQueryable<T> queryable,
            MethodInfo method
            )
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var linqExpression = queryable.Expression;

            var genericMethod = method.MakeGenericMethod(
                typeof(T)
                );

            var modifiedLinqExpression = System.Linq.Expressions.Expression.Call(
                genericMethod,
                linqExpression
                );

            return modifiedLinqExpression;
        }

    }
}