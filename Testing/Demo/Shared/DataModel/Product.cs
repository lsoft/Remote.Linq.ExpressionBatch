using System.Diagnostics;

namespace Shared.DataModel
{
    [DebuggerDisplay("{Id} {Name}")]
    public class Product
    {
        public int Id { get; set; }

        public int ProductCategoryId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public Product()
        {
            Id = 0;
            ProductCategoryId = 0;
            Name = string.Empty;
            Price = 0m;
        }

        public Product(
            int id,
            int productCategoryId,
            string name,
            decimal price
            )
        {
            Id = id;
            ProductCategoryId = productCategoryId;
            Name = name;
            Price = price;
        }

        public bool IsSame(
            Product other
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
            if (other.ProductCategoryId != ProductCategoryId)
            {
                return false;
            }
            if (other.Name != Name)
            {
                return false;
            }
            if (other.Price != Price)
            {
                return false;
            }

            return true;
        }
    }
}