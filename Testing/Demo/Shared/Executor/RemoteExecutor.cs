using System;
using RemoteLinq.ExpressionBatch.Executor;

namespace Shared.Executor
{
    public class RemoteExecutor : BaseRemoteExecutor
    {
        private readonly IRemoteService _grpcService;

        /// <inheritdoc />
        public RemoteExecutor(
            IRemoteService grpcService
            )
        {
            if (grpcService == null)
            {
                throw new ArgumentNullException(nameof(grpcService));
            }

            _grpcService = grpcService;
        }

        /// <inheritdoc />
        protected override byte[] SendReceive(
            byte[] request
            )
        {
            var replyBlob = _grpcService.ExecuteQuery(request);

            return replyBlob;
        }
    }
}