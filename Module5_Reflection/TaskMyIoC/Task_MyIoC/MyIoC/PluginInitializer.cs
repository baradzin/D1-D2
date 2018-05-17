using MyIoC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyIoC
{
    public class PluginInitializer
    {
        public PluginInitializer() { }      
        
        public IoC Initialize(Assembly assembly)
        {
            var container = new IoC(assembly);
            //RegisterAllExportedClasses(assembly, container);
            RegisterAllImportedClasses(assembly, container);
            return container;
        }

        private void RegisterAllExportedClasses(Assembly assembly, IoC container)
        {
            foreach (Type type in assembly.GetTypes())
            {
                var exportAtr = type.GetCustomAttributes(typeof(ExportAttribute), true).FirstOrDefault() 
                    as ExportAttribute;
                if (exportAtr!= null) {
                    if(exportAtr.Contract != null) {
                        container.Register(exportAtr.Contract, type);
                    } else {
                        container.Register(type);
                    }
                } 
            }
        }

        private void RegisterAllImportedClasses(Assembly assembly, IoC container)
        {
            foreach (Type type in assembly.GetTypes())
            {
                var importCtor = type.GetCustomAttributes(typeof(ImportConstructorAttribute), true).FirstOrDefault()
                    as ImportConstructorAttribute;
                if (importCtor != null) {
                    container.Register(type);
                } else {
                    var properties = type.GetProperties()
                        .Where(p => p.GetCustomAttribute(typeof(ImportAttribute), false) != null);
                    if (properties.Count() != 0) {
                        container.Register(type);
                    }
                }
            }
        }
    }
}
