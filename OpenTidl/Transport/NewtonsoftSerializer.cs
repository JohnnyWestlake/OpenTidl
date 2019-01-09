using JsonSubTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenTidl.Transport
{
    internal class NewtonsoftSerializer : IOpenTidlSerializer
    {
        JsonSerializer _defaultSerializer { get; } = new JsonSerializer
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        public TModel DeserializeObject<TModel>(Stream data) where TModel : class
        {
            using (var reader = new StreamReader(data))
            {
                return (TModel)_defaultSerializer.Deserialize(reader, typeof(TModel));
            }
        }
    }
}
