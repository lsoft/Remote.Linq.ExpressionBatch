using System;
using System.Collections.Generic;
using System.Linq;
using Shared.DataModel;

namespace Tests.DataModel
{
    public class TestServerDataModel : ITestServerDataModel
    {
        public static readonly TestSpecialException SpecialException = new("Expected exception");

        public IQueryable<Product> Products
        {
            get;
        } = new List<Product>
        {
            new Product { Id = 10, Name = "(tests) Apple", Price = 1m, ProductCategoryId = 1 },
            new Product { Id = 11, Name = "(tests) Pear", Price = 2m, ProductCategoryId = 1 },
            new Product { Id = 12, Name = "(tests) Pineapple", Price = 3m, ProductCategoryId = 1 },
            new Product { Id = 13, Name = "(tests) Car", Price = 33999m, ProductCategoryId = 2 },
            new Product { Id = 14, Name = "(tests) Bicycle", Price = 150m, ProductCategoryId = 2 },
        }.AsQueryable();

        public IQueryable<ProductCategory> ProductCategories
        {
            get;
        } = new List<ProductCategory>
        {
            new ProductCategory { Id = 1, Name = "(tests) Fruits" },
            new ProductCategory { Id = 2, Name = "(tests) Vehicles" },
        }.AsQueryable();

        public IQueryable<SpecialEntity> SpecialEntities
        {
            get;
        } = new List<SpecialEntity>
        {
            new SpecialEntity { Id = 1, RaiseException = false },
            new SpecialEntity { Id = 2, RaiseException = true, Exception = SpecialException },
        }.AsQueryable();


        public Func<Type, IQueryable> QueryableByTypeProvider =>
            (Type type) =>
            {
                // return wellknown entity sets.
                if (type == typeof(Product))
                {
                    return Products;
                }
                if (type == typeof(ProductCategory))
                {
                    return ProductCategories;
                }
                if (type == typeof(SpecialEntity))
                {
                    return SpecialEntities;
                }

                throw new InvalidOperationException(type.ToString());
            };
    }
}