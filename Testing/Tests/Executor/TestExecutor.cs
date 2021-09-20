using System;
using System.Threading.Tasks;
using Aqua.Dynamic;
using Remote.Linq.ExpressionExecution;
using Remote.Linq.Expressions;
using RemoteLinq.ExpressionBatch.Executor;
using RemoteLinq.ExpressionBatch.SerDe;
using Tests.DataModel;

namespace Tests.Executor
{
    public class TestExecutor : BaseRemoteExecutor
    {
        private readonly ITestServerDataModel _dataModel;

        /// <inheritdoc />
        public TestExecutor(
            ITestServerDataModel dataModel
            )
        {
            if (dataModel == null)
            {
                throw new ArgumentNullException(nameof(dataModel));
            }

            _dataModel = dataModel;
        }

        /// <inheritdoc />
        protected override Task<byte[]> SendReceiveAsync(
            byte[] request
            )
        {
            return Task.FromResult(SendReceive(request));
        }

        /// <inheritdoc />
        protected override byte[] SendReceive(
            byte[] request
            )
        {
            var expressions = FewSerDe.Deserialize<Expression>(request);

            var dos = new DynamicObject[expressions.Count];
            var index = 0;
            foreach (var expression in expressions)
            {
                var d_o = expression.Execute(_dataModel.QueryableByTypeProvider);

                dos[index] = d_o;
                index++;
            }

            var reply = FewSerDe.Serialize(dos);

            return reply;
        }
    }
}