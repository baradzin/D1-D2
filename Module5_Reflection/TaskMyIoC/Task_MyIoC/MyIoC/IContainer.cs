using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIoC
{
    public interface IContainer
    {
        void Register<TContract, TImplementation>();
        void Register<TImplementation>();
        T Resolve<T>() where T : class;
        Dictionary<Type, Type> GetAllRegisteredTypes();
    }
}