using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ringo.SaveSystem.DataLoading.Serialization.JSON
{
    public class JSONSaveDataBinder : ISerializationBinder
    {
        private Dictionary<string, Type> _typeMap;

        public JSONSaveDataBinder()
        {
            SetKnownTypes();
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            if (_typeMap.TryGetValue(typeName, out var type))
            {
                return type;
            }

            throw new JsonSerializationException($"Unknown type: '{typeName}' in assembly: '{assemblyName}'");
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = serializedType.Assembly.GetName().Name;

            var attribute = serializedType.GetCustomAttribute<Attributes.SaveData>();
            typeName = attribute?.Name ?? serializedType.FullName;
        }

        private void SetKnownTypes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var knownTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                Type[] types;
                
                try
                {
                    types = assembly.GetTypes();
                }
                // If any types in assembly cannot be loaded, grab all valid ones.
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray();
                }

                knownTypes.AddRange(from type in types let attr = type.GetCustomAttribute<Attributes.SaveData>() 
                    where attr != null && !string.IsNullOrEmpty(attr.Name) select type);
            }

            _typeMap = knownTypes.ToDictionary(
                t => t.GetCustomAttribute<Attributes.SaveData>().Name,
                t => t
            );
        }
    }
}
