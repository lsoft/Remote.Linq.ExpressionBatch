using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shared.DataModel;

namespace Server.DataModel
{
    public sealed partial class InMemoryDataStore : IServerDataModel
    {
        private readonly ProductCategory[] _productCategories;
        private readonly Product[] _products;

        public InMemoryDataStore()
        {
            _productCategories = new ProductCategory[]
            {
                new ProductCategory { Id = 1, Name = "(inmemory) Fruits" },
                new ProductCategory { Id = 2, Name = "(inmemory) Vehicles" },
            };

            _products = new Product[]
            {
                new Product { Id = 10, Name = "(inmemory) Apple", Price = 1m, ProductCategoryId = 1 },
                new Product { Id = 11, Name = "(inmemory) Pear", Price = 2m, ProductCategoryId = 1 },
                new Product { Id = 12, Name = "(inmemory) Pineapple", Price = 3m, ProductCategoryId = 1 },
                new Product { Id = 13, Name = "(inmemory) Car", Price = 33999m, ProductCategoryId = 2 },
                new Product { Id = 14, Name = "(inmemory) Bicycle", Price = 150m, ProductCategoryId = 2 },
            };
        }

        public IQueryable<ProductCategory> ProductCategories => _productCategories.AsQueryable();

        public IQueryable<Product> Products => _products.AsQueryable();

        public Func<Type, IQueryable> QueryableByTypeProvider => 
            (Type type) =>
             {
                 // return wellknown entity sets

                 if (type == typeof(ProductCategory))
                 {
                     return ProductCategories;
                 }

                 if (type == typeof(Product))
                 {
                     return Products;
                 }

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
