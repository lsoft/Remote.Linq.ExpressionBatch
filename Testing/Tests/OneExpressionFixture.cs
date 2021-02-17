using System;
using System.Collections.Generic;
using System.Linq;
using RemoteLinq.ExpressionBatch.Executor.Material;
using Shared.DataModel;
using Tests.DataModel;
using Tests.Executor;
using Tests.Helper;
using Xunit;

namespace Tests
{
    public class OneExpressionFixture
    {
        private readonly ITestServerDataModel _serverDataModel;
        private readonly TestExecutor _executor;
        private readonly ITestDataModel _clientDataModel;

        public OneExpressionFixture()
        {
            _serverDataModel = new TestServerDataModel();
            _executor = new TestExecutor(
                _serverDataModel
                );
            _clientDataModel = new TestClientDataModel();
        }

        [Fact]
        public void ToListUnknownType()
        {
            var queryable0 =
                from product in _clientDataModel.Products
                where product.Id < 12
                select new { ProductName = product.Name };

            var r0 = _executor.Execute(
                queryable0.AsList()
                );

            Assert.NotNull(r0);
            Assert.Equal(2, r0.Count());
            Assert.True(r0.First().ProductName == _serverDataModel.Products.First().Name);
            Assert.True(r0.Second().ProductName == _serverDataModel.Products.Second().Name);
        }

        [Fact]
        public void FirstOrDefaultUnknownType()
        {
            var queryable0 =
                from product in _clientDataModel.Products
                where product.Id < 12
                select new { ProductName = product.Name };

            var r0 = _executor.Execute(
                queryable0.AsFirstOrDefault()
                );

            Assert.NotNull(r0);
            Assert.True(r0.ProductName == _serverDataModel.Products.First().Name);
        }

        [Fact]
        public void ToList()
        {
            var queryable0 =
                from product in _clientDataModel.Products
                where product.Id < 12
                select product;

            var r0 = _executor.Execute(
                queryable0.AsList()
                );

            Assert.NotNull(r0);
            Assert.Equal(2, r0.Count());
            Assert.True(r0.First().IsSame(_serverDataModel.Products.First()));
            Assert.True(r0.Second().IsSame(_serverDataModel.Products.Second()));
        }

        [Fact]
        public void FirstOrDefault()
        {
            var queryable0 =
                from product in _clientDataModel.Products
                where product.Id < 12
                select product;

            var r0 = _executor.Execute(
                queryable0.AsFirstOrDefault()
                );

            Assert.NotNull(r0);
            Assert.True(r0.IsSame(_serverDataModel.Products.First()));
        }

        [Fact]
        public void FirstOrDefaultNoDataExists()
        {
            var queryable0 =
                from product in _clientDataModel.Products
                where product.Id == -1
                select product;

            var r0 = _executor.Execute(
                queryable0.AsFirstOrDefault()
                );

            Assert.Null(r0);
        }

        [Fact]
        public void First()
        {
            var queryable0 =
                from product in _clientDataModel.Products
                where product.Id < 12
                select product;

            var r0 = _executor.Execute(
                queryable0.AsFirst()
                );

            Assert.NotNull(r0);
            Assert.True(r0.IsSame(_serverDataModel.Products.First()));
        }

        [Fact]
        public void FirstNoDataExists()
        {
            var queryable0 =
                from product in _clientDataModel.Products
                where product.Id == -1
                select product;

            try
            {
                var r0 = _executor.Execute(
                    queryable0.AsFirst()
                    );

                this.ShouldFail("we're waited an exception");
            }
            catch (InvalidOperationException excp)
            {
                //it's ok
            }
        }

        /// <summary>
        /// For additional information please refer to https://github.com/6bee/Remote.Linq/issues/86
        /// </summary>
        [Fact]
        public void DefaultIfEmpty1()
        {
            var queryable0 = (
                    from product in _clientDataModel.Products
                    where product.Id == -1
                    select product
                    ).DefaultIfEmpty()
                ;

            var r0 = _executor.Execute(
                queryable0.AsList()
                );

            Assert.NotNull(r0);
            Assert.Single(r0);
            Assert.Null(r0.First());
        }

        [Fact]
        public void DefaultIfEmpty2()
        {
            var queryable0 = (
                from product in _clientDataModel.Products
                from category in _clientDataModel.ProductCategories.Where(c => c.Id == (product.ProductCategoryId + 1)).DefaultIfEmpty()
                select new ProductCategoryPair(product, category)
                )
                ;

            var r0 = _executor.Execute(
                queryable0.AsList()
                );

            Assert.NotNull(r0);
            Assert.Equal(5, r0.Count());
            Assert.Equal(3, r0.Count(p => p.Category is not null));
            Assert.Equal(2, r0.Count(p => p.Category is null));

            Assert.True(r0.Where(p => p.Category is not null).Any(p => p.Product.Id == 10));
            Assert.True(r0.Where(p => p.Category is not null).Any(p => p.Product.Id == 11));
            Assert.True(r0.Where(p => p.Category is not null).Any(p => p.Product.Id == 12));
            Assert.True(r0.Where(p => p.Category is null).Any(p => p.Product.Id == 13));
            Assert.True(r0.Where(p => p.Category is null).Any(p => p.Product.Id == 14));
        }

    }
}
