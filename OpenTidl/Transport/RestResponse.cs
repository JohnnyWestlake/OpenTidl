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
using System;
using System.Collections.Generic;
using OpenTidl.Models.Base;
using System.Runtime.Serialization.Json;
using System.IO;

namespace OpenTidl.Transport
{
    public class RestResponse<T> where T : ModelBase
    {
        #region properties

        public T Model { get; }
        public Int32 StatusCode { get; }
        public Exception Exception { get; }

        private static Dictionary<Type, DataContractJsonSerializer> _cache { get; } = new Dictionary<Type, DataContractJsonSerializer>();

        #endregion


        #region methods
        
        private TModel DeserializeObject<TModel>(Stream data) where TModel : class
        {
            return data == null 
                ? Activator.CreateInstance<TModel>() 
                : GetSerializer<TModel>().ReadObject(data) as TModel;
        }

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

        #endregion


        #region construction

        public RestResponse(Stream responseData, Int32 statusCode, String eTag)
        {
            if (statusCode < 300)
                this.Model = DeserializeObject<T>(responseData);
            if (statusCode >= 400)
                this.Exception = new OpenTidlException(DeserializeObject<ErrorModel>(responseData));
            this.StatusCode = statusCode;
            if (this.Model != null)
                (this.Model as ModelBase).ETag = eTag;
        }

        public RestResponse(Exception ex)
        {
            this.Exception = ex;
        }

        #endregion
    }
}
