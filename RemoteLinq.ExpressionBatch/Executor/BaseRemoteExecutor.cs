using System;
using Aqua.Dynamic;
using Remote.Linq.DynamicQuery;
using RemoteLinq.ExpressionBatch.Executor.Material;
using RemoteLinq.ExpressionBatch.SerDe;

namespace RemoteLinq.ExpressionBatch.Executor
{
    /// <summary>
    /// Remote expression executor that able to execute few expressions per each network request.
    /// It supports max 3 queries per request, but there is no problem to expand it further if necessary.
    /// </summary>
    public abstract class BaseRemoteExecutor
    {
        private readonly Remote.Linq.IQueryResultMapper<DynamicObject> _resultMapper;

        protected BaseRemoteExecutor()
        {
            _resultMapper = new DynamicResultMapper(null);
        }


        /// <summary>
        /// Execute one IQueryable per network request
        /// </summary>
        public T0 Execute<T0>(
            Materialized<T0> m0
            )
        {
            if (m0 == null)
            {
                throw new ArgumentNullException(nameof(m0));
            }

            var q0Expression = m0.Expression;

            var request = FewSerDe.Serialize(
                new[]
                {
                    Remote.Linq.DynamicQuery.ExpressionHelper.TranslateExpression(q0Expression, null, null),
                }
                );
            var reply = SendReceive(request);

            var dynamics = FewSerDe.Deserialize<DynamicObject>(reply);

            if (dynamics.Count != 1)
            {
                throw new InvalidOperationException("Should be 1 dynamic objects");
            }

            var result0 = _resultMapper.MapResult<T0>(dynamics[0], q0Expression);

            return result0;
        }

        /// <summary>
        /// Execute two IQueryable's per network request
        /// </summary>
        public (T0, T1) Execute<T0, T1>(
            Materialized<T0> m0,
            Materialized<T1> m1
            )
        {
            if (m0 == null)
            {
                throw new ArgumentNullException(nameof(m0));
            }

            if (m1 == null)
            {
                throw new ArgumentNullException(nameof(m1));
            }

            var q0Expression = m0.Expression;
            var q1Expression = m1.Expression;

            var request = FewSerDe.Serialize(
                new[]
                {
                    Remote.Linq.DynamicQuery.ExpressionHelper.TranslateExpression(q0Expression, null, null),
                    Remote.Linq.DynamicQuery.ExpressionHelper.TranslateExpression(q1Expression, null, null)
                }
                );
            var reply = SendReceive(request);

            var dynamics = FewSerDe.Deserialize<DynamicObject>(reply);

            if (dynamics.Count != 2)
            {
                throw new InvalidOperationException("Should be 2 dynamic objects");
            }

            var result0 = _resultMapper.MapResult<T0>(dynamics[0], q0Expression);
            var result1 = _resultMapper.MapResult<T1>(dynamics[1], q1Expression);

            return (result0, result1);
        }

        /// <summary>
        /// Execute three IQueryable's per network request
        /// </summary>
        public (T0, T1, T2) Execute<T0, T1, T2>(
            Materialized<T0> m0,
            Materialized<T1> m1,
            Materialized<T2> m2
            )
        {
            if (m0 == null)
            {
                throw new ArgumentNullException(nameof(m0));
            }

            if (m1 == null)
            {
                throw new ArgumentNullException(nameof(m1));
            }

            if (m2 == null)
            {
                throw new ArgumentNullException(nameof(m2));
            }

            var q0Expression = m0.Expression;
            var q1Expression = m1.Expression;
            var q2Expression = m2.Expression;

            var request = FewSerDe.Serialize(
                new[]
                {
                    Remote.Linq.DynamicQuery.ExpressionHelper.TranslateExpression(q0Expression, null, null),
                    Remote.Linq.DynamicQuery.ExpressionHelper.TranslateExpression(q1Expression, null, null),
                    Remote.Linq.DynamicQuery.ExpressionHelper.TranslateExpression(q2Expression, null, null)
                }
                );
            var reply = SendReceive(request);

            var dynamics = FewSerDe.Deserialize<DynamicObject>(reply);

            if (dynamics.Count != 3)
            {
                throw new InvalidOperationException("Should be 3 dynamic objects");
            }

            var result0 = _resultMapper.MapResult<T0>(dynamics[0], q0Expression);
            var result1 = _resultMapper.MapResult<T1>(dynamics[1], q1Expression);
            var result2 = _resultMapper.MapResult<T2>(dynamics[2], q2Expression);

            return (result0, result1, result2);
        }

        /// <summary>
        /// Perform network send-receive.
        /// </summary>
        protected abstract byte[] SendReceive(
            byte[] request
            );
    }
}
