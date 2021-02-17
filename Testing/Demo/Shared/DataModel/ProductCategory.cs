using System.Diagnostics;

namespace Shared.DataModel
{
    [DebuggerDisplay("{Id} {Name}")]
    public class ProductCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ProductCategory()
        {
            Id = 0;
            Name = string.Empty;
        }

        public ProductCategory(
            int id,
            string name
            )
        {
            Id = id;
            Name = name;
        }

        public bool IsSame(
            ProductCategory other
            )
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (other.Id != Id)
            {
                return false;
            }
            if (other.Name != Name)
            {
                return false;
            }

            return true;
        }
    }
}
