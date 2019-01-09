using OpenTidl.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace OpenTidl.Transport
{
    public class OpenTidlDataContractDeserializer : IOpenTidlSerializer
    {
        private static Dictionary<Type, DataContractJsonSerializer> _cache { get; } = new Dictionary<Type, DataContractJsonSerializer>();

        private DataContractJsonSerializer GetSerializer<TModel>() where TModel : class
        {
            Type t = typeof(TModel);
            if (!_cache.TryGetValue(t, out DataContractJsonSerializer serializer))
            {
                serializer = new DataContractJsonSerializer(t);
                _cache[t] = serializer;
            }

            return serializer;
        }

        public TModel DeserializeObject<TModel>(Stream data) where TModel : class
        {
            if (typeof(TModel) == typeof(EmptyModel) && data != null)
                return Activator.CreateInstance<TModel>();

            return data == null || data.Length == 0
                ? Activator.CreateInstance<TModel>()
                : GetSerializer<TModel>().ReadObject(data) as TModel;
        }
    }
}
