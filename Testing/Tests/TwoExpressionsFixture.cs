using System;
using System.Collections.Generic;
using System.Linq;
using RemoteLinq.ExpressionBatch.Executor.Material;
using Shared.DataModel;
using Tests.DataModel;
using Tests.Executor;
using Tests.Helper;
using Xunit;
using Xunit.Sdk;

namespace Tests
{
    public class TwoExpressionsFixture
    {
        private readonly ITestServerDataModel _serverDataModel;
        private readonly TestExecutor _executor;
        private readonly ITestDataModel _clientDataModel;

        public TwoExpressionsFixture()
        {
            _serverDataModel = new TestServerDataModel();
            _executor = new TestExecutor(
                _serverDataModel
                );
            _clientDataModel = new TestClientDataModel();
        }

        [Fact]
        public void ProductLess12()
        {
            var queryable0 =
                from product in _clientDataModel.Products
                where product.Id < 12
                select product;

            var (r0, r1) = _executor.Execute(
                queryable0.AsFirstOrDefault(),
                queryable0.AsList()
                );

            Assert.NotNull(r0);
            Assert.True(r0.IsSame(_serverDataModel.Products.First()));

            Assert.NotNull(r1);
            Assert.Equal(2, r1.Count());
            Assert.True(r1.First().IsSame(_serverDataModel.Products.First()));
            Assert.True(r1.Second().IsSame(_serverDataModel.Products.Second()));

        }

        [Fact]
        public void ProductHigher12()
        {
            var queryable0 =
                from product in _clientDataModel.Products
                where product.Id > 12
                select product;

            var (r0, r1) = _executor.Execute(
                queryable0.AsFirstOrDefault(),
                queryable0.AsList()
                );

            Assert.NotNull(r0);
            Assert.True(r0.IsSame(_serverDataModel.Products.ForwardBeyond(p => p.Id == 12).First()));

            Assert.NotNull(r1);
            Assert.Equal(2, r1.Count());
            Assert.True(r1.First().IsSame(_serverDataModel.Products.ForwardBeyond(p => p.Id == 12).First()));
            Assert.True(r1.Second().IsSame(_serverDataModel.Products.ForwardBeyond(p => p.Id == 12).Second()));

        }

        [Fact]
        public void ProductEquals12()
        {
            var queryable0 =
                from product in _clientDataModel.Products
                where product.Id == 12
                select product;

            var (r0, r1) = _executor.Execute(
                queryable0.AsFirstOrDefault(),
                queryable0.AsList()
                );

            Assert.NotNull(r0);
            Assert.True(r0.IsSame(_serverDataModel.Products.First(p => p.Id == 12)));

            Assert.NotNull(r1);
            Assert.Single(r1);
            Assert.True(r1.First().IsSame(r0));

        }

        [Fact]
        public void NoProduct()
        {
            var queryable0 =
                from product in _clientDataModel.Products
                where product.Id == -1
                select product;

            var (r0, r1) = _executor.Execute(
                queryable0.AsFirstOrDefault(),
                queryable0.AsList()
                );

            Assert.Null(r0);

            Assert.NotNull(r1);
            Assert.Empty(r1);

        }

        [Fact]
        public void ExceptionAtServer()
        {
            var queryable0 =
                from specialEntity in _clientDataModel.SpecialEntities
                where specialEntity.Id == 2
                select specialEntity;

            try
            { 
                var (r0, r1) = _executor.Execute(
                    queryable0.AsFirstOrDefault(),
                    queryable0.AsList()
                    );

                this.ShouldFail("we're waited an exception");
            }
            catch (TestSpecialException excp)
            when(excp.Message == TestServerDataModel.SpecialException.Message)
            {
                //it's ok
            }
        }

        [Fact]
        public void SelectCustomTypeEquals12()
        {
            var queryable0 =
                    from product in _clientDataModel.Products
                    from category in _clientDataModel.ProductCategories
                    where product.Id == 12 && product.ProductCategoryId == category.Id
                    select new ProductCategoryPair(product, category)
                ;

            var (r0, r1) = _executor.Execute(
                queryable0.AsFirstOrDefault(),
                queryable0.AsList()
                );

            var product12 = _serverDataModel.Products.First(p => p.Id == 12);
            var category12 = _serverDataModel.ProductCategories.First(c => c.Id == product12.ProductCategoryId);

            Assert.NotNull(r0);
            Assert.True(r0.Product.IsSame(product12));
            Assert.True(r0.Category.IsSame(category12));

            Assert.NotNull(r1);
            Assert.Single(r1);
            Assert.True(r1.First().IsSame(r0));
        }

    }
}
