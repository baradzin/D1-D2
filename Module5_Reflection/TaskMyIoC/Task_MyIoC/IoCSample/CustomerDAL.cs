using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attributes;

namespace Objects
{
    
    [Export(typeof(ICustomerDAL))]
    public class CustomerDAL : ICustomerDAL
    {
        public string yProperty { get; set; }
        public CustomerDAL()
        {
            this.yProperty  = "BLABLA";
        }
    }
}
