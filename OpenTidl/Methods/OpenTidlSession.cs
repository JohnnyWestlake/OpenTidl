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
using OpenTidl.Enums;
using OpenTidl.Models;
using OpenTidl.Models.Base;
using OpenTidl.Transport;

namespace OpenTidl.Methods
{
    public partial class OpenTidlSession
    {
        #region properties

        private OpenTidlClient _tidalClient { get; }
        private IRestClient _restClient { get; }

        private (string Key, string Value)[] _headers { get; }

        public LoginModel LoginResult { get; private set; }

        //FIXME: Throw error if empty
        public String SessionId     => LoginResult?.SessionId;
        public Int32 UserId         => LoginResult != null ? LoginResult.UserId : 0;
        public String CountryCode   => LoginResult?.CountryCode;

        #endregion


        #region opentidl methods


        #region logout methods

        public async Task<EmptyModel> LogoutAsync()
        {
            var result = await _restClient.HandleAsync<EmptyModel>("/logout", new
            {
                countryCode = CountryCode
            }, new { }, "POST", _headers);

            if (result == null )
                this.LoginResult = null; //Clear session

            return result;
        }

        #endregion

        private (string, string)[] Headers(params (string, string)[] headers)
        {
            if (headers == null || headers.Length == 0)
                return _headers;

            List<(string, string)> items = new List<(string, string)>(_headers);
            items.AddRange(headers);
            return items.ToArray();
        }

        private (string, string)[] Headers(string key, string value)
        {
            return new List<(string, string)>(_headers) { (key, value) }.ToArray();
        }


        #region playlist methods

        public Task<PlaylistModel> GetPlaylistAsync(String playlistUuid)
        {
            return _restClient.HandleAsync<PlaylistModel>(
                RestUtility.FormatUrl("/playlists/{uuid}", new { uuid = playlistUuid }), new
                {
                    countryCode = CountryCode
                }, null, "GET", _headers);
        }

        public Task<JsonList<TrackModel>> GetPlaylistTracksAsync(String playlistUuid, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return _restClient.HandleAsync<JsonList<TrackModel>>(
                RestUtility.FormatUrl("/playlists/{uuid}/tracks", new { uuid = playlistUuid }), new
                {
                    offset,
                    limit,
                    countryCode = CountryCode
                }, null, "GET", _headers);
        }

        public Task<EmptyModel> AddPlaylistTracksAsync(String playlistUuid, String playlistETag, IEnumerable<Int32> trackIds, Int32 toIndex = 0)
        {
            return _restClient.HandleAsync<EmptyModel>(
                RestUtility.FormatUrl("/playlists/{uuid}/tracks", new { uuid = playlistUuid }), new
                {
                    countryCode = CountryCode
                }, new
                {
                    trackIds = String.Join(",", trackIds),
                    toIndex
                }, "POST",
                Headers("If-None-Match", playlistETag));
        }

        public Task<EmptyModel> DeletePlaylistTracksAsync(String playlistUuid, String playlistETag, IEnumerable<Int32> indices)
        {
            return _restClient.HandleAsync<EmptyModel>(
                RestUtility.FormatUrl("/playlists/{uuid}/tracks/{indices}", new
                {
                    uuid = playlistUuid,
                    indices = String.Join(",", indices)
                }), new
                {
                    countryCode = CountryCode
                }, null, "DELETE",
                Headers("If-None-Match", playlistETag));
        }

        public Task<EmptyModel> DeletePlaylistAsync(String playlistUuid, String playlistETag)
        {
            return _restClient.HandleAsync<EmptyModel>(
                RestUtility.FormatUrl("/playlists/{uuid}", new
                {
                    uuid = playlistUuid
                }), new
                {
                    countryCode = CountryCode
                }, null, "DELETE",
                Headers("If-None-Match", playlistETag));
        }

        public Task<EmptyModel> MovePlaylistTracksAsync(String playlistUuid, String playlistETag, IEnumerable<Int32> indices, Int32 toIndex = 0)
        {
            return _restClient.HandleAsync<EmptyModel>(
                RestUtility.FormatUrl("/playlists/{uuid}/tracks/{indices}", new
                {
                    uuid = playlistUuid,
                    indices = String.Join(",", indices)
                }), new
                {
                    countryCode = CountryCode
                }, new
                {
                    toIndex
                }, "POST",
                Headers("If-None-Match", playlistETag));
        }

        public Task<EmptyModel> UpdatePlaylistAsync(String playlistUuid, String playlistETag, String title)
        {
            return _restClient.HandleAsync<EmptyModel>(
                RestUtility.FormatUrl("/playlists/{uuid}", new
                {
                    uuid = playlistUuid
                }), new
                {
                    countryCode = CountryCode
                }, new
                {
                    title
                }, "POST",
                Headers("If-None-Match", playlistETag));
        }

        #endregion


        #region session methods

        public Task<ClientModel> GetClientAsync()
        {
            return _restClient.HandleAsync<ClientModel>(
                RestUtility.FormatUrl("/sessions/{sessionId}/client", new { sessionId = SessionId }),
                null, null, "GET", _headers);
        }

        public Task<SessionModel> GetSessionAsync()
        {
            return _restClient.HandleAsync<SessionModel>(
                RestUtility.FormatUrl("/sessions/{sessionId}", new { sessionId = SessionId }),
                null, null, "GET", _headers);
        }

        #endregion


        #region track methods

        public Task<StreamUrlModel> GetTrackStreamUrlAsync(Int32 trackId, SoundQuality soundQuality, String playlistUuid)
        {
            return _restClient.HandleAsync<StreamUrlModel>(
                RestUtility.FormatUrl("/tracks/{id}/streamUrl", new { id = trackId }), new
                {
                    soundQuality,
                    playlistUuid,
                    countryCode = CountryCode
                }, null, "GET", _headers);
        }

        public Task<AssetModel> GetTrackAssetUrlAsync(int trackId)
        {
            return _restClient.HandleAsync<AssetModel>(
                RestUtility.FormatUrl("/tracks/{id}/urlpostpaywall", new { id = trackId }), new
                {
                    assetpresentation = "FULL",
                    audioquality = "HI_RES",
                    urlusagemode = "STREAM"
                }, null, "GET", _headers);
            //tracks/98068670/urlpostpaywall?assetpresentation=FULL&audioquality=HI_RES&urlusagemode=STREAM
        }

        public Task<StreamUrlModel> GetTrackOfflineUrlAsync(Int32 trackId, SoundQuality soundQuality)
        {
            return _restClient.HandleAsync<StreamUrlModel>(
                RestUtility.FormatUrl("/tracks/{id}/offlineUrl", new { id = trackId }), new
                {
                    soundQuality,
                    countryCode = CountryCode
                }, null, "GET", _headers);
        }

        #endregion


        #region user methods

        public Task<JsonList<ClientModel>> GetUserClientsAsync(ClientFilter filter, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return _restClient.HandleAsync<JsonList<ClientModel>>(
                RestUtility.FormatUrl("/users/{userId}/clients", new { userId = UserId }), new
                {
                    filter = filter.ToString(),
                    limit,
                    countryCode = CountryCode
                }, null, "GET", _headers);
        }

        public Task<JsonList<PlaylistModel>> GetUserPlaylistsAsync(Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return _restClient.HandleAsync<JsonList<PlaylistModel>>(
                RestUtility.FormatUrl("/users/{userId}/playlists", new { userId = UserId }), new
                {
                    limit,
                    countryCode = CountryCode
                }, null, "GET", _headers);
        }

        public Task<PlaylistModel> CreateUserPlaylistAsync(String title)
        {
            return _restClient.HandleAsync<PlaylistModel>(
                RestUtility.FormatUrl("/users/{userId}/playlists", new { userId = UserId }), new
                {
                    countryCode = CountryCode
                }, new
                {
                    title
                }, "POST", _headers);
        }

        public Task<UserSubscriptionModel> GetUserSubscriptionAsync()
        {
            return _restClient.HandleAsync<UserSubscriptionModel>(
                RestUtility.FormatUrl("/users/{userId}/subscription", new { userId = UserId }), new
                {
                    countryCode = CountryCode
                }, null, "GET", _headers);
        }

        public Task<UserModel> GetUserAsync()
        {
            return _restClient.HandleAsync<UserModel>(
                RestUtility.FormatUrl("/users/{userId}", new { userId = UserId }), new
                {
                    countryCode = CountryCode
                }, null, "GET", _headers);
        }

        #endregion


        #region user favorites methods

        public Task<JsonList<JsonListItem<AlbumModel>>> GetFavoriteAlbumsAsync(
            int offset = 0,
            int limit = 100,
            SortOrder order = SortOrder.DATE,
            SortDirection direction = SortDirection.DESC)
        {
            return _restClient.HandleAsync<JsonList<JsonListItem<AlbumModel>>>(
                RestUtility.FormatUrl("/users/{userId}/favorites/albums", new { userId = UserId }), new
                {
                    offset,
                    limit,
                    order = order.ToString("F"),
                    orderDirection = direction.ToString("F"),
                    countryCode = CountryCode
                }, null, "GET", _headers);
        }

        public Task<JsonList<JsonListItem<ArtistModel>>> GetFavoriteArtistsAsync(
            int offset = 0,
            int limit = 100,
            SortOrder order = SortOrder.DATE,
            SortDirection direction = SortDirection.DESC)
        {
            return _restClient.HandleAsync<JsonList<JsonListItem<ArtistModel>>>(
                RestUtility.FormatUrl("/users/{userId}/favorites/artists", new { userId = UserId }), new
                {
                    offset,
                    limit,
                    order = order.ToString("F"),
                    orderDirection = direction.ToString("F"),
                    countryCode = CountryCode
                }, null, "GET", _headers);
        }

        public Task<JsonList<JsonListItem<PlaylistModel>>> GetFavoritePlaylistsAsync(Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return _restClient.HandleAsync<JsonList<JsonListItem<PlaylistModel>>>(
                RestUtility.FormatUrl("/users/{userId}/favorites/playlists", new { userId = UserId }), new
                {
                    limit,
                    countryCode = CountryCode
                }, null, "GET", _headers);
        }

        public Task<JsonList<JsonListItem<TrackModel>>> GetFavoriteTracksAsync(
            int offset = 0, 
            int limit = 100, 
            SortOrder order = SortOrder.DATE,
            SortDirection direction = SortDirection.DESC)
        {
            return _restClient.HandleAsync<JsonList<JsonListItem<TrackModel>>>(
                RestUtility.FormatUrl("/users/{userId}/favorites/tracks", new { userId = UserId }), new
                {
                    offset,
                    limit,
                    order = order.ToString("F"),
                    orderDirection = direction.ToString("F"),
                    countryCode = CountryCode
                }, null, "GET", _headers);
        }

        public Task<EmptyModel> AddFavoriteAlbumAsync(Int32 albumId)
        {
            return _restClient.HandleAsync<EmptyModel>(
                RestUtility.FormatUrl("/users/{userId}/favorites/albums", new { userId = UserId }), 
                new { countryCode = CountryCode }, 
                new { albumId }, 
                "POST", _headers);
        }

        public Task<EmptyModel> AddFavoriteArtistAsync(Int32 artistId)
        {
            return _restClient.HandleAsync<EmptyModel>(
                RestUtility.FormatUrl("/users/{userId}/favorites/artists", new { userId = UserId }), new
                {
                    countryCode = CountryCode
                }, new
                {
                    artistId
                }, "POST", _headers);
        }

        public Task<EmptyModel> AddFavoritePlaylistAsync(String playlistUuid)
        {
            return _restClient.HandleAsync<EmptyModel>(
                RestUtility.FormatUrl("/users/{userId}/favorites/playlists", new { userId = UserId }), new
                {
                    countryCode = CountryCode
                }, new
                {
                    uuid = playlistUuid
                }, "POST", _headers);
        }

        public Task<EmptyModel> AddFavoriteTrackAsync(Int32 trackId)
        {
            return _restClient.HandleAsync<EmptyModel>(
                RestUtility.FormatUrl("/users/{userId}/favorites/tracks", new { userId = UserId }), new
                {
                    countryCode = CountryCode
                }, new
                {
                    trackId
                }, "POST", _headers);
        }

        public Task<EmptyModel> RemoveFavoriteAlbumAsync(Int32 albumId)
        {
            return _restClient.HandleAsync<EmptyModel>(
                RestUtility.FormatUrl("/users/{userId}/favorites/albums/{albumId}", new 
                { 
                    userId = UserId,
                    albumId
                }), new
                {
                    countryCode = CountryCode
                }, null, "DELETE", _headers);
        }

        public Task<EmptyModel> RemoveFavoriteArtistAsync(Int32 artistId)
        {
            return _restClient.HandleAsync<EmptyModel>(
                RestUtility.FormatUrl("/users/{userId}/favorites/artists/{artistId}", new
                {
                    userId = UserId,
                    artistId
                }), new
                {
                    countryCode = CountryCode
                }, null, "DELETE", _headers);
        }

        public Task<EmptyModel> RemoveFavoritePlaylistAsync(String playlistUuid)
        {
            return _restClient.HandleAsync<EmptyModel>(
                RestUtility.FormatUrl("/users/{userId}/favorites/playlists/{uuid}", new
                {
                    userId = UserId,
                    uuid = playlistUuid
                }), new
                {
                    countryCode = CountryCode
                }, null, "DELETE", _headers);
        }

        public Task<EmptyModel> RemoveFavoriteTrackAsync(Int32 trackId)
        {
            return _restClient.HandleAsync<EmptyModel>(
                RestUtility.FormatUrl("/users/{userId}/favorites/tracks/{trackId}", new
                {
                    userId = UserId,
                    trackId
                }), new
                {
                    countryCode = CountryCode
                }, null, "DELETE", _headers);
        }

        #endregion


        #region video methods

        public Task<VideoModel> GetVideoAsync(Int32 videoId)
        {
            return _restClient.HandleAsync<VideoModel>(
                RestUtility.FormatUrl("/videos/{id}", new { id = videoId }), new
                {
                    countryCode = CountryCode
                }, null, "GET", _headers);
        }

        public Task<VideoStreamUrlModel> GetVideoStreamUrlAsync(Int32 videoId, VideoQuality videoQuality)
        {
            return _restClient.HandleAsync<VideoStreamUrlModel>(
                RestUtility.FormatUrl("/videos/{id}/streamurl", new { id = videoId }), new
                {
                    videoQuality,
                    sessionId = SessionId,
                    countryCode = CountryCode
                }, null, "GET", _headers);
        }

        #endregion


        #endregion


        #region methods

        private T HandleResponse<T>(RestResponse<T> response) where T : ModelBase
        {
            return this._tidalClient.HandleResponse(response);
        }

        #endregion


        #region construction

        internal OpenTidlSession(OpenTidlClient client, LoginModel loginModel, IRestClient restClient)
        {
            this._tidalClient = client;
            this.LoginResult = loginModel;
            this._restClient = restClient;
            _headers = new (string, string)[] { ("X-Tidal-SessionId", loginModel?.SessionId) };
        }

        #endregion
    }
}
