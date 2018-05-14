using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIoC
{
    public interface IContainer
    {
        void Register<TTypeToResolve, TConcrete>();
        void Register<TTypeToResolve, TConcrete>(LifeCycle lifeCycle);
        TTypeToResolve Resolve<TTypeToResolve>();
        object Resolve(Type typeToResolve);
    }

    public class SimpleIocContainer : IContainer
    {

        private readonly IList registeredObjects = new List();

        public void Register<TTypeToResolve, TConcrete>()

        {

            Register<TTypeToResolve, TConcrete>(LifeCycle.Singleton);

        }



        public void Register<TTypeToResolve, TConcrete>(LifeCycle lifeCycle)

        {

            registeredObjects.Add(new RegisteredObject(typeof(TTypeToResolve), typeof(TConcrete), lifeCycle));

        }



        public TTypeToResolve Resolve()

        {

            return (TTypeToResolve)ResolveObject(typeof(TTypeToResolve));

        }



        public object Resolve(Type typeToResolve)

        {

            return ResolveObject(typeToResolve);

        }



        private object ResolveObject(Type typeToResolve)

        {

            var registeredObject = registeredObjects.FirstOrDefault(o => o.TypeToResolve == typeToResolve);

            if (registeredObject == null)

            {

                throw new TypeNotRegisteredException(string.Format(

                    "The type {0} has not been registered", typeToResolve.Name));

            }

            return GetInstance(registeredObject);

        }



        private object GetInstance(RegisteredObject registeredObject)

        {

            if (registeredObject.Instance == null ||

                registeredObject.LifeCycle == LifeCycle.Transient)

            {

                var parameters = ResolveConstructorParameters(registeredObject);

                registeredObject.CreateInstance(parameters.ToArray());

            }

            return registeredObject.Instance;

        }



        private IEnumerable<object> ResolveConstructorParameters(RegisteredObject registeredObject)

        {

            var constructorInfo = registeredObject.ConcreteType.GetConstructors().First();

            foreach (var parameter in constructorInfo.GetParameters())

            {

                yield return ResolveObject(parameter.ParameterType);

            }

        }
    }
