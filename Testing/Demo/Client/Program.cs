//#define LOCAL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Grpc.Core;
using ProtoBuf.Grpc.Client;
using Remote.Linq.Expressions;
using RemoteLinq.ExpressionBatch.Executor;
using RemoteLinq.ExpressionBatch.Executor.Material;
using Shared;
using Shared.DataModel;
using Shared.Executor;

namespace Client
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Client is running");

            var channel = new Channel("localhost", 10040/*10050*/, ChannelCredentials.Insecure);
            try
            {
                //RLINQ

#if LOCAL
                var executor = new FakeExecutor(
                    );
#else
                var executor = new RemoteExecutor(
                    channel.CreateGrpcService<IRemoteService>()
                    );
#endif

                IRemoteDataModel dataModel = new ClientDataModel();

                var queryable0 =
                    from product in dataModel.Products
                    where product.Id < 12
                    select product;

                var queryable1 =
                    from product in dataModel.Products
                    where product.Id > 12
                    select product;

                var (r0, r1) = executor.Execute(
                    queryable0.AsFirstOrDefault(),
                    queryable1.AsFirstOrDefault()
                    );

                var (er0, er1) = executor.Execute(
                    queryable0.AsList(),
                    queryable1.AsList()
                    );

                var (gr0, gr1) = executor.Execute(
                    queryable0.AsFirstOrDefault(),
                    queryable1.AsList()
                    );

                var queryable2 =
                        from product in dataModel.Products
                        from category in dataModel.ProductCategories
                        where product.Id == 12 && product.ProductCategoryId == category.Id
                        select new ProductCategoryPair(product, category)
                    ;

                var (hr0, hr1) = executor.Execute(
                    queryable2.AsFirstOrDefault(),
                    queryable2.AsList()
                    );

                var queryable3 =
                    from product in dataModel.Products
                    where product.Id != product.Id
                    select product;

                var (jr0, jr1) = executor.Execute(
                    queryable3.AsFirstOrDefault(),
                    queryable3.AsFirstOrDefault()
                    );


                var (kr0, kr1) = executor.Execute(
                    queryable3.AsList(),
                    queryable3.AsList()
                    );

                var (lr0, lr1) = executor.Execute(
                    queryable3.AsList(),
                    queryable3.AsFirstOrDefault()
                    );

                ////CLASSIC
                //Console.WriteLine("--------------- CLASSIC ---------------");

                //var response = remoteService.GetAllProductCategories(
                //    new ProductCategoriesRequest()
                //    );

                //foreach (var category in response.ProductCategories)
                //{
                //    Console.WriteLine($"{category.Id} : {category.Name}");
                //}

                Console.WriteLine($"Done");
                Console.ReadLine();
            }
            finally
            {
                channel.ShutdownAsync().Wait();
            }
        }
    }
}
