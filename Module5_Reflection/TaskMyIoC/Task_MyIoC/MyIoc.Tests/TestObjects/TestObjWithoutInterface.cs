using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIoc.Tests.TestObjects
{
    public class TestObjWithoutInterface
    {
        public TestLogger Logger { get; set; }

        public TestObject TestObj { get; set; }

        public TestObjWithoutInterface(TestObject tO, TestLogger tL)
        {
            this.Logger = tL;
            this.TestObj = tO;
        }
    }
}
