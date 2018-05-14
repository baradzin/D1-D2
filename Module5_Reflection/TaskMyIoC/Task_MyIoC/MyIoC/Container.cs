using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyIoC.Objects;

namespace MyIoC
{
	public class Container
	{
        public Assembly Asm { get; private set; }


        public void AddAssembly(Assembly assembly)
		{
            Asm = assembly;
        }

		public void AddType(Type type)
		{ }

		public void AddType(Type type, Type baseType)
		{ }

		public object CreateInstance(Type type)
		{
			return null;
		}

		public T CreateInstance<T>()
		{
			return default(T);
		}


		public void Sample()
		{
			var container = new Container();
			container.AddAssembly(Assembly.GetExecutingAssembly());

			var customerBLL = (CustomerBLL_CTOR)container.CreateInstance(typeof(CustomerBLL_CTOR));
			var customerBLL2 = container.CreateInstance<CustomerBLL_CTOR>();

			container.AddType(typeof(CustomerBLL_CTOR));
			container.AddType(typeof(Logger));
			container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));
		}
	}
}
