using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using MyIoC.Objects;

namespace MyIoC
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            IoC container = new IoC(Assembly.GetExecutingAssembly());
            container.Register<CustomerBLL_CTOR>();
            container.Register<Logger>();
            container.Register<ICustomerDAL, CustomerDAL>();

            var ctorBLL = container.Resolve<CustomerBLL_CTOR>();
            var bbb = container.Resolve<ICustomerDAL>();
        }
    }
    //public class Container
    //{
    //    public Assembly Asm { get; private set; }


    //    public void AddAssembly(Assembly assembly)
    //    {
    //        Asm = assembly;
    //    }

    //    public void AddType(Type type)
    //    { }

    //    public void AddType(Type type, Type baseType)
    //    { }

    //    public object CreateInstance(Type type)
    //    {
    //        return null;
    //    }

    //    public T CreateInstance<T>()
    //    {
    //        return default(T);
    //    }


    //    public void Sample()
    //    {
    //        IoC container = new IoC(Assembly.GetExecutingAssembly());
    //        container.Register<CustomerBLL_CTOR>();
    //        container.Register<Logger>();
    //        container.Register<ICustomerDAL, CustomerDAL>();
    //        var customerCtor = contain

    //        var container = new Container();
    //        container.AddAssembly(Assembly.GetExecutingAssembly());

    //        var customerBLL = (CustomerBLL_CTOR)container.CreateInstance(typeof(CustomerBLL_CTOR));
    //        var customerBLL2 = container.CreateInstance<CustomerBLL_CTOR>();

    //        container.AddType(typeof(CustomerBLL_CTOR));
    //        container.AddType(typeof(Logger));
    //        container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));
    //    }
    //}
}
