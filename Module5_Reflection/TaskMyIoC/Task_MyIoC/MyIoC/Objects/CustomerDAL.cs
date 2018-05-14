using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyIoC.Attributes;

namespace MyIoC.Objects
{
    [Export(typeof(ICustomerDAL))]
    public class CustomerDAL : ICustomerDAL
    { }
}
