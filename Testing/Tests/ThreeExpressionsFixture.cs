using System.Linq;
using RemoteLinq.ExpressionBatch.Executor.Material;
using Tests.DataModel;
using Tests.Executor;
using Xunit;

namespace Tests
{
    public class ThreeExpressionsFixture
    {
        private readonly ITestServerDataModel _serverDataModel;
        private readonly TestExecutor _executor;
        private readonly ITestDataModel _clientDataModel;

        public ThreeExpressionsFixture()
        {
            _serverDataModel = new TestServerDataModel();
            _executor = new TestExecutor(
                _serverDataModel
                );
            _clientDataModel = new TestClientDataModel();
        }

        [Fact]
        public void ProductId11Id12Id13()
        {
            var queryable0 =
                from product in _clientDataModel.Products
                where product.Id == 11
                select product;
            var queryable1 =
                from product in _clientDataModel.Products
                where product.Id == 12
                select product;
            var queryable2 =
                from product in _clientDataModel.Products
                where product.Id == 13
                select product;

            var (r0, r1, r2) = _executor.Execute(
                queryable0.AsFirstOrDefault(),
                queryable1.AsFirstOrDefault(),
                queryable2.AsFirstOrDefault()
                );

            Assert.NotNull(r0);
            Assert.True(r0.IsSame(_serverDataModel.Products.First(p => p.Id == 11)));

            Assert.NotNull(r1);
            Assert.True(r1.IsSame(_serverDataModel.Products.First(p => p.Id == 12)));
            
            Assert.NotNull(r2);
            Assert.True(r2.IsSame(_serverDataModel.Products.First(p => p.Id == 13)));
        }
    }
}
