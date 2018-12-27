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
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace OpenTidl.Transport
{
    public class OpenTidlNetworkClient : INetworkClient
    {
        HttpClient _client { get; } = new HttpClient();

        public async Task<StreamResponse> GetResponseStreamAsync(string url, string method, byte[] content, bool allowCache, List<(string, string)> headers)
        {
            var req = CreateRequest(url, method, headers);
            ByteArrayContent bc = null;
            if (content != null)
            {
                bc = new ByteArrayContent(content);
                bc.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                req.Content = bc;
            }

            HttpResponseMessage response;
            try
            {
                response = await _client.SendAsync(req).ConfigureAwait(false);
                var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                return new StreamResponse(stream, (Int32)response.StatusCode, response.Headers.ETag?.Tag);
            }
            //catch (HttpRequestException webEx)
            //{
            //    response = webEx.
            //}
            catch (Exception ex)
            {
                return new StreamResponse(ex);
            }
            finally
            {
                bc?.Dispose();
            }
        }

        private HttpRequestMessage CreateRequest(String url, String method, List<(String Key, String Value)> headers)
        {
            var req = new HttpRequestMessage(new HttpMethod(method), url);

            if (headers != null)
            {
                foreach (var (Key, Value) in headers)
                    req.Headers.TryAddWithoutValidation(Key, Value);
            }
            return req;
        }
    }
}
