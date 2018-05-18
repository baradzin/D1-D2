using Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyIoC.Container
{
    public class IoC : IContainer
    {
        //private Assembly _asm;

        public Dictionary<Type, Type> registeredTypes =
            new Dictionary<Type, Type>();

        public IoC() { }

        /// <summary>
        /// Register types
        /// </summary>
        /// <typeparam name="TContract">From type</typeparam>
        /// <typeparam name="TImplementation">Call this implementation</typeparam>
        /// <returns>returns the IoC container</returns>
        public void Register<TContract, TImplementation>()
        {
            Register(typeof(TContract), typeof(TImplementation));
        }

        /// <summary>
        /// Register types without Contract
        /// </summary>
        /// <typeparam name="TImplementation">Call this implementation</typeparam>
        /// <returns>returns the IoC container</returns>
        public void Register<TImplementation>()
        {
            Register(typeof(TImplementation), typeof(TImplementation));
        }

        public void Register(Type contract, Type implementation)
        {
            var tmp = contract;
            if (registeredTypes.ContainsKey(tmp))
                registeredTypes[tmp] = implementation;
            else
            {
                registeredTypes.Add(tmp, implementation);
            }
        }
        
        public void Register(Type implementation)
        {
            Register(implementation, implementation);
        }

        public void Register(Assembly assembly)
        {
            RegisterAllExportedClasses(assembly);
            RegisterAllImportedClasses(assembly);
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

        public object Resolve(Type type)
        {
            if (!registeredTypes.Any())
                throw new Exception("No entity has been registered yet.");
            return ResolveParameter(type);
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
                bool flag = registeredTypes.ContainsKey(type);
                //WTF, return not value
                Type resolved = registeredTypes.ContainsKey(type) ? registeredTypes[type] : type;

                //Check for initialization via properties
                var properties = type.GetProperties()
                        .Where(p => p.GetCustomAttribute(typeof(ImportAttribute), false) != null);
                if(properties.Count()!= 0) {
                    var obj = Activator.CreateInstance(resolved);
                    foreach(var prop in properties) {
                        prop.SetValue(obj, Resolve(prop.PropertyType));
                    }
                    return obj;
                }
                
                var ctor = resolved.GetConstructors().First();
                var ctorParams = ctor.GetParameters().Where(w => w.GetType().IsClass);

                // If constructor hasn't parameter, Create an instance of object
                //Resolve?
                if (!ctorParams.Any())
                    return Activator.CreateInstance(resolved);

                var paramLst = new List<object>(ctorParams.Count());

                // Iterate through parameters and resolve each parameter
                for (int i = 0; i < ctorParams.Count(); i++)
                {
                    var paramType = ctorParams.ElementAt(i).ParameterType;
                    var resolvedParam = ResolveParameter(paramType);
                    paramLst.Add(resolvedParam);
                }

                return ctor.Invoke(paramLst.ToArray());
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

        private void RegisterAllExportedClasses(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                var exportAtr = type.GetCustomAttributes(typeof(ExportAttribute), true).FirstOrDefault()
                    as ExportAttribute;
                if (exportAtr != null)
                {
                    if (exportAtr.Contract != null)
                    {
                        Register(exportAtr.Contract, type);
                    }
                    else
                    {
                        Register(type);
                    }
                }
            }
        }

        private void RegisterAllImportedClasses(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                var importCtor = type.GetCustomAttributes(typeof(ImportConstructorAttribute), true).FirstOrDefault()
                    as ImportConstructorAttribute;
                if (importCtor != null)
                {
                    Register(type);
                }
                else
                {
                    var properties = type.GetProperties()
                        .Where(p => p.GetCustomAttribute(typeof(ImportAttribute), false) != null);
                    if (properties.Count() != 0)
                    {
                        Register(type);
                    }
                }
            }
        }
    }
}
 
