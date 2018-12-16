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
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.IO;
using OpenTidl.Models.Base;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;

namespace OpenTidl.Transport
{
    internal class RestClient
    {
        #region properties

        internal String ApiEndpoint { get; private set; }
        internal KeyValuePair<String, String>[] Headers { get; private set; }

        #endregion


        #region methods

        HttpClient client { get; }

        internal async Task<RestResponse<T>> ProcessAsync<T>(String path, Object query, Object request, String method, params KeyValuePair<String, String>[] extraHeaders) where T : ModelBase
        {
            var encoding = new UTF8Encoding(false);
            var queryString = RestUtility.GetFormEncodedString(query);
            var url = String.IsNullOrEmpty(queryString) ? String.Format("{0}{1}", ApiEndpoint, path) : 
                String.Format("{0}{1}?{2}", ApiEndpoint, path, queryString);
            var req = CreateRequest(url, method, extraHeaders);
            if (request != null)
            {
                req.Content = new FormUrlEncodedContent(RestUtility.GetFormEncodedList(request));
            }

            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(req).ConfigureAwait(false);

                var str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return new RestResponse<T>(str, (Int32)response.StatusCode, response.Headers.ETag?.Tag);

            }
            //catch (HttpRequestException webEx)
            //{
            //    response = webEx.
            //}
            catch (Exception ex)
            {
                return new RestResponse<T>(ex);
            }
        }

        internal async Task<HttpResponseMessage> GetWebResponseAsync(String url)
        {
            var req = CreateRequest(url, "GET", null);
            try
            {
                var resp = await client.SendAsync(req).ConfigureAwait(false);
                return resp;
            }
            catch
            {
                return null;
            }
        }

        internal async Task<WebStreamModel> GetWebStreamModelAsync(String url)
        {
            var req = CreateRequest(url, "GET", null);
            try
            {
                var resp = await client.SendAsync(req).ConfigureAwait(false);
                return await WebStreamModel.CreateAsync(resp).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        private HttpRequestMessage CreateRequest(String url, String method, KeyValuePair<String, String>[] extraHeaders)
        {
            var req = new HttpRequestMessage(new HttpMethod(method), url);
            
            if (extraHeaders != null)
            {
                foreach (var h in extraHeaders)
                    req.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }
            return req;
        }

        #endregion


        #region construction

        internal RestClient(String apiEndpoint, String userAgent, params KeyValuePair<String, String>[] headers)
        {
            this.ApiEndpoint = apiEndpoint ?? "";

            client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(userAgent))
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            }

            client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip");
            client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("defalte");

            if (headers != null)
            {
                foreach (var h in headers)
                    client.DefaultRequestHeaders.TryAddWithoutValidation(h.Key, h.Value);
            }
        }

        #endregion
    }
}
