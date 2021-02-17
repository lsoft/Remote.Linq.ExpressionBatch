using System;
using System.Linq;
using Aqua.Dynamic;
using Remote.Linq;
using Remote.Linq.Expressions;
using Shared;
using Shared.DataModel;
using Shared.Executor;

namespace Client
{
    public class ClientDataModel : IRemoteDataModel
    {
        /// <inheritdoc />
        public IQueryable<ProductCategory> ProductCategories =>
            RemoteQueryable.Factory.CreateQueryable<ProductCategory>(DataProvider);

        /// <inheritdoc />
        public IQueryable<Product> Products =>
            RemoteQueryable.Factory.CreateQueryable<Product>(DataProvider);

        private static DynamicObject DataProvider(Expression _)
            => throw new NotSupportedException($"Please use {nameof(RemoteExecutor)} to execute expressions.");
    }
}
