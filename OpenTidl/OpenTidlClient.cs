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
using OpenTidl.Transport;
using System.IO;
using OpenTidl.Models.Base;
using OpenTidl.Models;
using OpenTidl.Methods;
using OpenTidl.Enums;
using System.Net;

namespace OpenTidl
{
    public partial class OpenTidlClient
    {
        #region fields

        private const String FALLBACK_COUNTRY_CODE = "US";
        private string _defaultCountryCode;

        #endregion


        #region properties

        public ClientConfiguration Configuration { get; private set; }
        private IRestClient RestClient { get; set; }

        private String LastSessionCountryCode { get; set; }
        private String DefaultCountryCode { get { return _defaultCountryCode; } }

        #endregion


        #region methods
        
        internal KeyValuePair<String, String> Header(String header, String value)
        {
            return new KeyValuePair<String, String>(header, value);
        }

        internal T HandleResponse<T>(RestResponse<T> response) where T : ModelBase
        {
            if (response.Exception != null)
                throw response.Exception;
            return response.Model;
        }

        private LoginModel HandleLoginResponse(RestResponse<LoginModel> response, LoginType loginType)
        {
            if (response.Exception != null)
                throw response.Exception;
            var model = response.Model;
            if (model != null)
            {
                this.LastSessionCountryCode = model.CountryCode;
                model.LoginType = loginType;
            }
            return model;
        }

        private SessionModel HandleSessionResponse(RestResponse<SessionModel> response)
        {
            if (response.Exception != null)
                throw response.Exception;
            var model = response.Model;
            if (model != null)
                this.LastSessionCountryCode = model.CountryCode;
            return model;
        }

        private String GetCountryCode()
        {
            return !String.IsNullOrEmpty(LastSessionCountryCode) ? LastSessionCountryCode : DefaultCountryCode;
        }

        private async Task<String> GetDefaultCountryCodeAsync()
        {
            CountryModel cc = null;
            try
            {
                cc = await this.GetCountryAsync().ConfigureAwait(false); ;
            }
            catch { }
            if (cc != null && !String.IsNullOrEmpty(cc.CountryCode))
                return cc.CountryCode;
            if (Configuration != null && !String.IsNullOrEmpty(Configuration.DefaultCountryCode))
                return Configuration.DefaultCountryCode;
            return FALLBACK_COUNTRY_CODE;
        }

        #endregion


        #region construction

        public static async Task<OpenTidlClient> TryCreateAsync(ClientConfiguration config)
        {
            var client = new OpenTidlClient(config);
            client._defaultCountryCode = (await client.GetCountryAsync()).CountryCode;
            return client;
        }

        private OpenTidlClient(ClientConfiguration config, IRestClient client = null)
        {
            this.Configuration = config;
            this.RestClient = client ?? new RestClient(config.ApiEndpoint, config.UserAgent, Header("X-Tidal-Token", config.Token));
        }

        #endregion
    }
}
