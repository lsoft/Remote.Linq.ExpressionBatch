using System.Linq;
using Shared.DataModel;

namespace Tests.DataModel
{
    public interface ITestDataModel
    {
        IQueryable<ProductCategory> ProductCategories
        {
            get;
        }

        IQueryable<Product> Products
        {
            get;
        }

        IQueryable<SpecialEntity> SpecialEntities
        {
            get;
        }
    }
}