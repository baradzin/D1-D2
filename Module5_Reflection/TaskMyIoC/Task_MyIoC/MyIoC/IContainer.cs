using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIoC.Container
{
    public interface IContainer
    {
        void Register(Type contract, Type implementation, bool isConstructorInjection);
        void Register(Type implementation, bool isConstructorInjection);
        T Resolve<T>() where T : class;
        Dictionary<Type, Tuple<Type, bool>> GetAllRegisteredTypes();
    }
}