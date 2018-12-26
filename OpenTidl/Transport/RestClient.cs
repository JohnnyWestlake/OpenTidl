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
using System.Text;
using System.Threading.Tasks;
using OpenTidl.Models.Base;
using System.Net.Http;
using System.Linq;
using System.Net.Http.Headers;

namespace OpenTidl.Transport
{
    public interface IRestClient
    {
        Task<T> HandleAsync<T>(String path, Object query, Object request, String method, params (string, string)[] extraHeaders) where T : ModelBase;
        Task<RestResponse<T>> GetResponseAsync<T>(String path, Object query, Object request, String method, params (string, string)[] extraHeaders) where T : ModelBase;
        Task<WebStreamModel> GetWebStreamModelAsync(String url);
    }

    internal class RestClient : IRestClient
    {
        #region properties

        internal String ApiEndpoint { get; private set; }
        internal List<(string, string)> Headers { get; private set; }

        #endregion


        #region methods

        HttpClient _client { get; }

        public async Task<T> HandleAsync<T>(String path, Object query, Object request, String method, params (String, String)[] extraHeaders) where T : ModelBase
        {
            var response = await GetResponseAsync<T>(path, query, request, method, extraHeaders).ConfigureAwait(false);

            if (response.Exception != null)
                throw response.Exception;

            return response.Model;
        }

        public async Task<RestResponse<T>> GetResponseAsync<T>(String path, Object query, Object request, String method, params (String, String)[] extraHeaders) where T : ModelBase
        {
            var queryString = RestUtility.GetFormEncodedString(query);
            var url = String.IsNullOrEmpty(queryString) ? $"{ApiEndpoint}{path}" : $"{ApiEndpoint}{path}?{queryString}";
            byte[] content = null;

            if (RestUtility.GetFormEncodedList(request) is var data && data != null)
            {
                using (var form = new FormUrlEncodedContent(data))
                {
                    content = await form.ReadAsByteArrayAsync().ConfigureAwait(false);
                }
            }

            var headers = Headers.ToList();
            headers.AddRange(extraHeaders);

            return await SendAsync<T>(url, method, content, headers).ConfigureAwait(false);
        }

        public async Task<RestResponse<T>> SendAsync<T>(string url, string method, byte[] content, List<(string, string)> headers) where T : ModelBase
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
                using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    return new RestResponse<T>(stream, (Int32)response.StatusCode, response.Headers.ETag?.Tag);
                }
            }
            //catch (HttpRequestException webEx)
            //{
            //    response = webEx.
            //}
            catch (Exception ex)
            {
                return new RestResponse<T>(ex);
            }
            finally
            {
                bc?.Dispose();
            }
        }

        public async Task<WebStreamModel> GetWebStreamModelAsync(String url)
        {
            var req = CreateRequest(url, "GET", null);
            try
            {
                var resp = await _client.SendAsync(req).ConfigureAwait(false);
                return await WebStreamModel.CreateAsync(resp).ConfigureAwait(false);
            }
            catch
            {
                return null;
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

        #endregion


        #region construction

        public RestClient(String apiEndpoint, String userAgent, params KeyValuePair<String, String>[] headers)
        {
            this.ApiEndpoint = apiEndpoint ?? "";

            _client = new HttpClient();
            Headers = new List<(string, string)>
            {
                ("User-Agent", userAgent),
                ("Accept-Encoding", "gzip, deflate")
            };

            if (headers != null)
            {
                foreach (var h in headers)
                    Headers.Add((h.Key, h.Value));
            }
        }

        #endregion
    }
}
