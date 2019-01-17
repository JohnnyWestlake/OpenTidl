/*
    Copyright (C) 2019 J. Westlake

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
*/

using System;
using System.IO;
using System.Threading.Tasks;
using OpenTidl.Models.Base;

namespace OpenTidl.Transport
{
    public interface IRestClient
    {
        Task<T> HandleAsync<T>(String path, Object query, Object request, String method, bool canCache = true, params (string, string)[] extraHeaders) where T : class;
        Task<RestResponse<T>> GetResponseAsync<T>(String path, Object query, Object request, String method, bool canCache, params (string, string)[] extraHeaders) where T : class;
        Task<WebStreamModel> GetWebStreamModelAsync(String url);
        void SetSerializer(IOpenTidlSerializer serializer);
    }
}
