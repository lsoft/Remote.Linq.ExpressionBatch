using System.Collections.Generic;
using ProtoBuf;

namespace Shared.DataModel
{
    [ProtoContract]
    public class ProductCategoriesReply
    {
        [ProtoMember(1)]
        public List<ProductCategoryResult> ProductCategories { get; set; }

        public ProductCategoriesReply()
        {
            ProductCategories = new List<ProductCategoryResult>();
        }
    }
}