using System;
using System.Diagnostics;

namespace Shared.DataModel
{
    [DebuggerDisplay("P:{Product} C:{Category}")]
    public class ProductCategoryPair
    {
        public Product Product { get; set; }

        public ProductCategory? Category { get; set; }

        public ProductCategoryPair(
            Product product,
            ProductCategory? category
            )
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            Product = product;
            Category = category;
        }

        public bool IsSame(
            ProductCategoryPair other
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

            if (!Product.IsSame(other.Product))
            {
                return false;
            }

            if (Category is null && other.Category is not null)
            {
                return false;
            }
            if (Category is not null && other.Category is null)
            {
                return false;
            }

            if (Category is not null && other.Category is not null)
            {
                if (!Category.IsSame(other.Category))
                {
                    return false;
                }
            }

            return true;
        }
    }
}