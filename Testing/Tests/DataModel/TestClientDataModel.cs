using System;
using System.Linq;
using Aqua.Dynamic;
using Remote.Linq;
using Remote.Linq.Expressions;
using Shared.DataModel;

namespace Tests.DataModel
{
    public class TestClientDataModel : ITestDataModel
    {
        /// <inheritdoc />
        public IQueryable<ProductCategory> ProductCategories =>
            RemoteQueryable.Factory.CreateQueryable<ProductCategory>(
                new Func<Expression, DynamicObject>((e) => null)
                );

        /// <inheritdoc />
        public IQueryable<Product> Products =>
            RemoteQueryable.Factory.CreateQueryable<Product>(
                new Func<Expression, DynamicObject>((e) => null)
                );

        /// <inheritdoc />
        public IQueryable<SpecialEntity> SpecialEntities =>
            RemoteQueryable.Factory.CreateQueryable<SpecialEntity>(
                new Func<Expression, DynamicObject>((e) => null)
                );
    }
}
