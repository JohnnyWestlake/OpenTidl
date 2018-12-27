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
using System.Linq;
using System.Net.NetworkInformation;

namespace OpenTidl
{
    public class ClientConfiguration
    {
        #region properties

        public String ApiEndpoint { get; }
        public String UserAgent { get; private set; }
        public String Token { get; }
        public String ClientUniqueKey { get; private set; }
        public String ClientVersion { get; }
        public String DefaultCountryCode { get; private set; }

        #endregion


        #region methods

        /// <summary>
        /// Creates a default ClientConfiguration to access the tidal API
        /// </summary>
        /// <param name="tidalToken">Required. A token uniquely identifying your tidal application.</param>
        /// <param name="clientKey">Optional. A uniquely generated key identifying the client. If not specified, a default implementation is used.</param>
        /// <param name="clientVersion">Optional. Provide an informational client version string.</param>
        /// <param name="defaultCountry">Optional. Override the default country code. This may be automatically override by Tidal API.</param>
        /// <returns></returns>
        public static ClientConfiguration CreateDefault(string tidalToken, string clientKey = null, string clientVersion = "1.19.0.0", string defaultCountry = "US")
        {
            if (string.IsNullOrWhiteSpace(tidalToken))
                throw new ArgumentNullException(nameof(tidalToken));

            return new ClientConfiguration("https://api.tidalhifi.com/v1", null, tidalToken, clientKey ?? DefaultClientUniqueKey, clientVersion, defaultCountry);
        }


        private static String DefaultClientUniqueKey
        {
            get
            {
                var macAddress = NetworkInterface.GetAllNetworkInterfaces().Where(i => 
                    i.OperationalStatus == OperationalStatus.Up && i.NetworkInterfaceType != NetworkInterfaceType.Loopback).OrderByDescending(i => 
                        i.Speed).Select(i => i.GetPhysicalAddress().GetAddressBytes()).FirstOrDefault();
                if (macAddress == null)
                    return "123456789012345";
                return String.Join("", macAddress.Skip(1).Select(b => b.ToString("000")));
            }
        }

        #endregion


        #region construction

        public ClientConfiguration(String apiEndpoint, String userAgent, String token, String clientUniqueKey, String clientVersion, String defaultCountryCode)
        {
            this.ApiEndpoint = apiEndpoint;
            this.UserAgent = userAgent;
            this.Token = token;
            this.ClientUniqueKey = clientUniqueKey;
            this.ClientVersion = clientVersion;
            this.DefaultCountryCode = defaultCountryCode;
        }

        #endregion
    }
}
