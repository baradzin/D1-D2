using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyIoC.Attributes;

namespace MyIoC.Objects
{
    [ImportConstructor]
    public class CustomerBLL_CTOR
    {
        public CustomerBLL_CTOR(ICustomerDAL dal, Logger logger)
        { }
    }
}
