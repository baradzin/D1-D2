using Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIoc.Tests.TestObjects
{
    [Export]
    public class TestLogger
    {
        public string Example { get; set; }

        public TestLogger()
        {
            this.Example = "Test Logger EXPORT";
        }
    }
}
