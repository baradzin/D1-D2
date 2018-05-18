using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Collections.Generic;
using Attributes;
using System.Linq;
using MyIoC.Container;

namespace MyIoc.Tests
{
    [TestClass]
    public class ExternalAssemblyTest
    {
        private Assembly asm = Assembly.LoadFrom(@"D:\D1-D2\Module5_Reflection\TaskMyIoC\Task_MyIoC\IoCSample\bin\Debug\IoCSample.dll");

        [TestMethod]
        public void InitiateAllComponents()
        {
            IoC container = new IoC();
            container.Register(asm);
            List<object> objects = new List<object>();

            foreach(var pair in container.registeredTypes)
            {
                objects.Add(container.Resolve(pair.Key));
            }
        }

        [TestMethod]
        public void CheckInstancesOfImportConstructor()
        {
            IoC container = new IoC();
            container.Register(asm);

            foreach (Type type in asm.GetTypes())
            {
                var importCtorAtr = type.GetCustomAttributes(typeof(ImportConstructorAttribute), true).FirstOrDefault()
                    as ImportConstructorAttribute;
                if (importCtorAtr != null)
                {
                    var instance = container.Resolve(type);
                    Assert.AreEqual(type, instance.GetType());
                }
            }
        }

        [TestMethod]
        public void CheckInstancesOfExportContract()
        {
            IoC container = new IoC();
            container.Register(asm);

            foreach (Type type in asm.GetTypes())
            {
                var exportAtr = type.GetCustomAttributes(typeof(ExportAttribute), true).FirstOrDefault()
                    as ExportAttribute;
                if (exportAtr != null)
                {
                    if(exportAtr.Contract != null)
                    {
                        var instance = container.Resolve(type);
                        Assert.IsTrue(type.IsAssignableFrom(instance.GetType()));
                    }
                }
            }
        }
    }
}
