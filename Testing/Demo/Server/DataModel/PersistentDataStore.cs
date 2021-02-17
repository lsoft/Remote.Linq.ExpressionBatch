using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;
using LinqToDB.Mapping;
using Shared.DataModel;

namespace Server.DataModel
{
    public sealed class PersistentDataStore : DataConnection, IServerDataModel
    {

        private static MappingSchema? _schema;

        public PersistentDataStore(
            string connectionString
            )
            :base(
                 new SqlServerDataProvider("MSSQL", SqlServerVersion.v2012),
                 connectionString,
                 CreateMapping())
        {
        }

        public IQueryable<Product> Products => GetTable<Product>();

        public IQueryable<ProductCategory> ProductCategories => GetTable<ProductCategory>();

        private static MappingSchema CreateMapping()
        {
            if (_schema == null) // TODO: multithreading
            {
                _schema = new MappingSchema();

                var m = _schema.GetFluentMappingBuilder();

                m
                    .Entity<Product>()
                    .HasTableName(nameof(Products))
                    .Property(x => x.Id).HasColumnName(nameof(Product.Id)).IsIdentity().IsPrimaryKey()
                    .Property(x => x.Name).HasColumnName(nameof(Product.Name)).IsNullable(false)
                    .Property(x => x.Price).HasColumnName(nameof(Product.Price)).IsNullable(false)
                    .Property(x => x.ProductCategoryId).HasColumnName(nameof(Product.ProductCategoryId)).IsNullable(false)
                    ;

                m
                    .Entity<ProductCategory>()
                    .HasTableName(nameof(ProductCategories))
                    .Property(x => x.Id).HasColumnName(nameof(ProductCategory.Id)).IsIdentity().IsPrimaryKey()
                    .Property(x => x.Name).HasColumnName(nameof(ProductCategory.Name)).IsNullable(false)
                    ;

            }
            return _schema;
        }

        public Func<Type, IQueryable> QueryableByTypeProvider =>
            (Type type) =>
            {
                // return wellknown entity sets

                //if (type == typeof(ProductGroup))
                //{
                //    return ProductGroups;
                //}

                if (type == typeof(ProductCategory))
                {
                    return ProductCategories;
                }

                if (type == typeof(Product))
                {
                    return Products;
                }

                //if (type == typeof(OrderItem))
                //{
                //    return OrderItems;
                //}

                // return entity sets possibly declared in partial class
                var queryableType = typeof(IEnumerable<>).MakeGenericType(type);
                var dataset = GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(p => queryableType.IsAssignableFrom(p.PropertyType))
                    .FirstOrDefault()?
                    .GetValue(this) as IEnumerable;
                return dataset?.AsQueryable() ?? throw new NotSupportedException($"No queryable resource available for type {type}");
            };

    }
}
