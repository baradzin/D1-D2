using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attributes;

namespace IoCSample
{
    [ImportConstructor]
    public class CustomerBLL_CTOR
    {
        public ICustomerDAL DAL { get; set; }

        public Logger Loger { get; set; }

        public CustomerBLL_CTOR(ICustomerDAL dal, Logger logger)
        {
            this.DAL = dal;
            this.Loger = logger;
        }
    }
}
