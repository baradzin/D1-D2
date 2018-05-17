using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIoC
{
    public interface IContainer
    {
        void Register(Type contract, Type implementation);
        void Register(Type implementation);
        T Resolve<T>() where T : class;
        Dictionary<Type, Type> GetAllRegisteredTypes();
    }
}