using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public partial class Supplier : IEquatable<Supplier>
    {
        public override bool Equals(object supplier)
        {
            var item = supplier as Supplier;

            if (item != null)
            {
                return this.Equals(item);
            }

            return false;
        }

        public bool Equals(Supplier other)
        {
            return this.CompanyName.Equals(other.CompanyName);
        }

        public override int GetHashCode()
        {
            return this.SupplierID.GetHashCode();
        }
    }
}
