using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public partial class Category : IEquatable<Category>
    {
        public override bool Equals(object category)
        {
            var item = category as Category;

            if (item != null) {
                return this.Equals(item);
            }

            return false;
        }

        public bool Equals(Category other)
        {
            return this.CategoryName.Equals(other.CategoryName);
        }

        public override int GetHashCode()
        {
            return this.CategoryID.GetHashCode();
        }
    }
}
