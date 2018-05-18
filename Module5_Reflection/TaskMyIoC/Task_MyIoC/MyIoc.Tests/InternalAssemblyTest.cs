using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyIoc.Tests.TestObjects;
using MyIoC.Container;
using System.Collections.Generic;
using System.Reflection;

namespace MyIoc.Tests
{
    [TestClass]
    public class InternalAssemblyTest
    {
        private Assembly asm = Assembly.GetExecutingAssembly();

        [TestMethod]
        public void InitFromCurrentAssembly()
        {
            IoC container = new IoC();
            container.Register(asm);

            TestObject testObj = container.Resolve<TestObject>();
            Assert.AreEqual(testObj.IProperty, default(string));

            TestLogger logger = container.Resolve<TestLogger>();
            Assert.AreEqual(logger.Example, "Test Logger EXPORT");
        }

        [TestMethod]
        public void GetInstanceWithoutAttribute()
        {
            IoC container = new IoC();
            container.Register(asm);
            container.Register<TestObjWithoutInterface>();

            var towi = (TestObjWithoutInterface)container.Resolve(typeof(TestObjWithoutInterface));
            Assert.AreEqual(towi.Logger.GetType(), typeof(TestLogger));
            Assert.AreEqual(towi.Logger.Example, "Test Logger EXPORT");
        }

        [TestMethod]
        public void RegisterInstanceForInterface()
        {
            IoC container = new IoC();
            container.Register<ITest, TestObject>();

            var obj = container.Resolve(typeof(ITest));
            Assert.AreEqual(obj.GetType(), typeof(TestObject));
        }
    }
}
