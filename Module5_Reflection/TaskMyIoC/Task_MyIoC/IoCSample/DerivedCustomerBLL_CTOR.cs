using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attributes;

namespace IoCSample
{
    [ImportConstructor]
    public class DerivedCustomerBLL_CTOR : CustomerBLL_CTOR
    {
        public DerivedCustomerBLL_CTOR(ICustomerDAL dal, Logger log) : base(dal,log)
        {

        }
    }
}
