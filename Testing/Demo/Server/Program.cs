//#define SQL

using System;
using System.Data;
using System.Linq;
using Grpc.Core;
using LinqToDB;
using Microsoft.Data.SqlClient;
using ProtoBuf.Grpc.Server;
using Server.DataModel;
using Shared;
using Shared.DataModel;

namespace Server
{
    class Program
    {
        public static void Main(string[] args)
        {
#if SQL
            var connectionString = args[0];

            using (var ctx = new PersistentDataStore(connectionString))
            {
                using (var tran = ctx.BeginTransaction())
                {
                    using (IDbCommand cmd = new SqlCommand($"truncate table dbo.Products"))
                    {
                        cmd.Transaction = tran.DataConnection.Transaction;
                        cmd.Connection = tran.DataConnection.Connection;
                        cmd.ExecuteNonQuery();
                    }
                    using (IDbCommand cmd = new SqlCommand($"truncate table dbo.ProductCategories"))
                    {
                        cmd.Transaction = tran.DataConnection.Transaction;
                        cmd.Connection = tran.DataConnection.Connection;
                        cmd.ExecuteNonQuery();
                    }


                    ctx.InsertWithInt32Identity(
                        new ProductCategory { Id = 1, Name = "(sql) Fruits" }
                        );
                    ctx.InsertWithInt32Identity(
                        new ProductCategory { Id = 2, Name = "(sql) Vehicles" }
                        );

                    ctx.InsertWithInt32Identity(
                        new Product { Id = 10, Name = "(sql) Apple", Price = 1m, ProductCategoryId = 1 }
                        );
                    ctx.InsertWithInt32Identity(
                        new Product { Id = 11, Name = "(sql) Pear", Price = 2m, ProductCategoryId = 1 }
                        );
                    ctx.InsertWithInt32Identity(
                        new Product { Id = 12, Name = "(sql) Pineapple", Price = 3m, ProductCategoryId = 1 }
                        );
                    ctx.InsertWithInt32Identity(
                        new Product { Id = 13, Name = "(sql) Car", Price = 33999m, ProductCategoryId = 2 }
                        );
                    ctx.InsertWithInt32Identity(
                        new Product { Id = 14, Name = "(sql) Bicycle", Price = 150m, ProductCategoryId = 2 }
                        );

                    tran.Commit();
                }
            }
#endif

            Grpc.Core.Server server = new Grpc.Core.Server
            {
                Ports = { new ServerPort("localhost", 10040, ServerCredentials.Insecure) },
            };

            var calc = new RemoteServiceImplementation(
#if SQL 
                new PersistentDataStore(connectionString)
#else
                new InMemoryDataStore()
#endif
                );
            server.Services.AddCodeFirst<IRemoteService>(calc);

            server.Start();
            Console.WriteLine("Server running... press any key");
            Console.ReadKey();
            server.ShutdownAsync().Wait();
        }

    }
}
