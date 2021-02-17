using System;
using System.Collections.Generic;
using System.IO;
using Aqua.Dynamic;
using Remote.Linq.Expressions;
using RemoteLinq.ExpressionBatch.SerDe;
using Server.DataModel;
using Shared;
using Shared.DataModel;

namespace Server
{
    public class RemoteServiceImplementation : IRemoteService
    {
        private readonly IServerDataModel _serverDataModel;

        public RemoteServiceImplementation(
            IServerDataModel serverDataModel
            )
        {
            if (serverDataModel == null)
            {
                throw new ArgumentNullException(nameof(serverDataModel));
            }

            _serverDataModel = serverDataModel;
        }

        /// <inheritdoc />
        public byte[] ExecuteQuery(
            byte[] requestBlob
            )
        {
            var expressions = FewSerDe.Deserialize<Expression>(requestBlob);

            var dos = new DynamicObject?[expressions.Count];
            var index = 0;
            foreach (var expression in expressions)
            {
                if (expression is null)
                {
                    throw new InvalidOperationException("Incoming expression somehow is null");
                }

                var d_o = expression.Execute(
                    _serverDataModel.QueryableByTypeProvider
                    );
                dos[index] = d_o;
                index++;
            }

            var reply = FewSerDe.Serialize(dos);

            return reply;
        }

        public ProductCategoriesReply GetAllProductCategories(ProductCategoriesRequest request)
        {
            throw new NotImplementedException("This is only a demonstation that a standard grpc-net calls works");
        }
    }
}
