using ProtoBuf;

namespace Shared.DataModel
{
    [ProtoContract]
    public class ProductCategoryResult
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        public ProductCategoryResult()
        {
            Id = 0;
            Name = string.Empty;
        }

        public ProductCategoryResult(
            int id,
            string name
            )
        {
            Id = id;
            Name = name;
        }
    }
}