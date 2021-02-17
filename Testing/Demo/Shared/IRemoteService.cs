using System.ServiceModel;
using Shared.DataModel;

namespace Shared
{
    [ServiceContract]
    public interface IRemoteService
    {
        byte[] ExecuteQuery(
            byte[] query
            );


        ProductCategoriesReply GetAllProductCategories(ProductCategoriesRequest request);
    }
}