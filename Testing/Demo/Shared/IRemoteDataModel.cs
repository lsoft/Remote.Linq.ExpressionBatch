using System.Linq;
using Shared.DataModel;

namespace Shared
{
    public interface IRemoteDataModel
    {
        IQueryable<ProductCategory> ProductCategories
        {
            get;
        }

        IQueryable<Product> Products
        {
            get;
        }
    }
}
