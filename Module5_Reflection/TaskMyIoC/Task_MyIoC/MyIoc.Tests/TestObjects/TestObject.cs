using Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIoc.Tests.TestObjects
{
    [Export(typeof(ITest))]
    public class TestObject : ITest
    {
        public string IProperty { get ; set ;}
    }
}
