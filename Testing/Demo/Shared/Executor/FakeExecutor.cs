using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aqua.Dynamic;
using Remote.Linq.ExpressionExecution;
using Remote.Linq.Expressions;
using RemoteLinq.ExpressionBatch.Executor;
using RemoteLinq.ExpressionBatch.SerDe;
using Shared.DataModel;

namespace Shared.Executor
{
    public class FakeExecutor : BaseRemoteExecutor
    {
        private readonly FakeDataModel _dataModel;

        /// <inheritdoc />
        public FakeExecutor(
            )
        {
            _dataModel = new FakeDataModel();
        }

        /// <summary>
        /// Gets Todo.
        /// </summary>
        public Func<Type, IQueryable> QueryableByTypeProvider =>
            (Type type) =>
            {
                // return wellknown entity sets.
                if (type == typeof(Product))
                {
                    return _dataModel.Products;
                }
                if (type == typeof(ProductCategory))
                {
                    return _dataModel.ProductCategories;
                }

                throw new InvalidOperationException(type.ToString());
            };

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

            var dos = new DynamicObject?[expressions.Count];
            var index = 0;
            foreach (var expression in expressions)
            {
                if (expression is null)
                {
                    throw new InvalidOperationException("Incoming expression somehow is null");
                }

                var d_o = expression.Execute(QueryableByTypeProvider);

                dos[index] = d_o;
                index++;
            }

            var reply = FewSerDe.Serialize(dos);

            return reply;
        }

        private class FakeDataModel : IRemoteDataModel
        {
            public IQueryable<Product> Products
            {
                get;
            } = new List<Product>
                {
                    new Product { Id = 10, Name = "(fake) Apple", Price = 1m, ProductCategoryId = 1 },
                    new Product { Id = 11, Name = "(fake) Pear", Price = 2m, ProductCategoryId = 1 },
                    new Product { Id = 12, Name = "(fake) Pineapple", Price = 3m, ProductCategoryId = 1 },
                    new Product { Id = 13, Name = "(fake) Car", Price = 33999m, ProductCategoryId = 2 },
                    new Product { Id = 14, Name = "(fake) Bicycle", Price = 150m, ProductCategoryId = 2 },
                }.AsQueryable();

            public IQueryable<ProductCategory> ProductCategories
            {
                get;
            } = new List<ProductCategory>
            {
                new ProductCategory { Id = 1, Name = "(fake) Fruits" },
                new ProductCategory { Id = 2, Name = "(fake) Vehicles" },
            }.AsQueryable();
        }
    }
}