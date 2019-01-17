/*
    Copyright (C) 2015  Jack Fagner

    This file is part of OpenTidl.

    OpenTidl is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    OpenTidl is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with OpenTidl.  If not, see <http://www.gnu.org/licenses/>.

    --- 

    Modified 2019 J. Westlake
*/


using OpenTidl.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

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
