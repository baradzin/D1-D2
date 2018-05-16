using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyIoC
{
    public class IoC : IContainer
    {
        private Assembly _asm;

        Dictionary<Type, Type> registeredTypes =
            new Dictionary<Type, Type>();

        //Dictionary<Type, object> instances =
        //    new Dictionary<Type, object>();

        public IoC(Assembly assembly)
        {
            this._asm = assembly;
        }

        /// <summary>
        /// Register types
        /// </summary>
        /// <typeparam name="TContract">From type</typeparam>
        /// <typeparam name="TImplementation">Call this implementation</typeparam>
        /// <returns>returns the IoC container</returns>
        public void Register<TContract, TImplementation>()
        {
            var tmp = typeof(TContract);
            if (registeredTypes.ContainsKey(tmp))
                registeredTypes[tmp] = typeof(TImplementation);
            else
            {
                registeredTypes.Add(tmp, typeof(TImplementation));
            }
        }

        /// <summary>
        /// Register types without Contract
        /// </summary>
        /// <typeparam name="TImplementation">Call this implementation</typeparam>
        /// <returns>returns the IoC container</returns>
        public void Register<TImplementation>()
        {
            Register<TImplementation, TImplementation>();
        }

        /// <summary>
        /// Create instance of target Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class
        {
            if (!registeredTypes.Any())
                throw new Exception("No entity has been registered yet.");

            T targetObject = (T)ResolveParameter(typeof(T));

            return targetObject;
        }

        /// <summary>
        /// Create instance of object
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private object ResolveParameter(Type type)
        {
            try
            {
                Type resolved = registeredTypes[type];

                var cnstr = resolved.GetConstructors().First();
                var cnstrParams = cnstr.GetParameters().Where(w => w.GetType().IsClass);

                // If constructor hasn't parameter, Create an instance of object
                if (!cnstrParams.Any())
                    return Activator.CreateInstance(resolved);

                var paramLst = new List<object>(cnstrParams.Count());

                // Iterate through parameters and resolve each parameter
                for (int i = 0; i < cnstrParams.Count(); i++)
                {
                    var paramType = cnstrParams.ElementAt(i).ParameterType;
                    var resolvedParam = ResolveParameter(paramType);
                    paramLst.Add(resolvedParam);
                }

                return cnstr.Invoke(paramLst.ToArray());
            }
            catch (Exception)
            {
                var err = string.Format("'{0}' Cannot be resolved. Check your registered types", type.FullName);
                throw new Exception(err);
            }

        }

        /// <summary>
        /// Get all of the registered types for this container
        /// </summary>
        /// <returns></returns>
        public Dictionary<Type, Type> GetAllRegisteredTypes()
        {
            return registeredTypes;
        }

        /// <summary>
        /// Returns true if specified type were registered
        /// </summary>
        /// <typeparam name="Type">The type that you want to check if is registered in the container</typeparam>
        /// <returns>True/False</returns>
        public bool IsRegistered<Type>()
        {
            return registeredTypes.Any(a => a.Key == typeof(Type));
        }
    }
}
 
