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
using OpenTidl.Models;
using OpenTidl.Models.Base;
using OpenTidl.Enums;

namespace OpenTidl
{
    public partial class OpenTidlClient
    {

        #region album methods

        public Task<AlbumModel> GetAlbumAsync(Int32 albumId)
        {
            return RestClient.HandleAsync<AlbumModel>(
                $"/albums/{albumId}", new
                {
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<ModelArray<AlbumModel>> GetAlbumsAsync(IEnumerable<Int32> albumIds)
        {
            return RestClient.HandleAsync<ModelArray<AlbumModel>>(
                "/albums", new
                {
                    ids = String.Join(",", albumIds),
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<AlbumModel>> GetSimilarAlbumsAsync(Int32 albumId)
        {
            return RestClient.HandleAsync<JsonList<AlbumModel>>(
                $"/albums/{albumId}/similar", new
                {
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<TrackModel>> GetAlbumTracksAsync(Int32 albumId)
        {
            return RestClient.HandleAsync<JsonList<TrackModel>>(
                $"/albums/{albumId}/tracks", new
                {
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<CreditLink>> GetAlbumTracksWithCreditsAsync(Int32 albumId, int offset = 0, int limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<CreditLink>>(
                $"/albums/{albumId}/items/credits", new
                {
                    replace = true,
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<AlbumReviewModel> GetAlbumReviewAsync(Int32 albumId)
        {
            return RestClient.HandleAsync<AlbumReviewModel>(
                $"/albums/{albumId}/review", new
                {
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        #endregion


        #region artist methods

        public Task<ArtistModel> GetArtistAsync(Int32 artistId)
        {
            return RestClient.HandleAsync<ArtistModel>(
                $"/artists/{artistId}", new
                {
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<AlbumModel>> GetArtistAlbumsAsync(
            int artistId, AlbumFilter filter, int offset = 0, int limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<AlbumModel>>(
                $"/artists/{artistId}/albums", new
                {
                    filter = filter.ToString("F"),
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<TrackModel>> GetRadioFromArtistAsync(Int32 artistId, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<TrackModel>>(
                $"/artists/{artistId}/radio", new
                {
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<ArtistModel>> GetSimilarArtistsAsync(Int32 artistId, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<ArtistModel>>(
                $"/artists/{artistId}/similar", new
                {
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<TrackModel>> GetArtistTopTracksAsync(Int32 artistId, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<TrackModel>>(
                $"/artists/{artistId}/toptracks", new
                {
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<VideoModel>> GetArtistVideosAsync(Int32 artistId, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<VideoModel>>(
                $"/artists/{artistId}/videos", new
                {
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<ArtistBiographyModel> GetArtistBiographyAsync(Int32 artistId)
        {
            return RestClient.HandleAsync<ArtistBiographyModel>(
                $"/artists/{artistId}/bio", new
                {
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<LinkModel>> GetArtistLinksAsync(Int32 artistId, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<LinkModel>>(
                $"/artists/{artistId}/links", new
                {
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        #endregion


        #region country methods

        public Task<CountryModel> GetCountryAsync()
        {
            return RestClient.HandleAsync<CountryModel>("/country", null, null, "GET");
        }

        #endregion

        
        #region search methods

        public Task<JsonList<AlbumModel>> SearchAlbumsAsync(String query, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<AlbumModel>>(
                "/search/albums", new
                {
                    query,
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<ArtistModel>> SearchArtistsAsync(String query, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<ArtistModel>>(
                "/search/artists", new
                {
                    query,
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<PlaylistModel>> SearchPlaylistsAsync(String query, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<PlaylistModel>>(
                "/search/playlists", new
                {
                    query,
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<TrackModel>> SearchTracksAsync(String query, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<TrackModel>>(
                "/search/tracks", new
                {
                    query,
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<VideoModel>> SearchVideosAsync(String query, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<VideoModel>>(
                "/search/videos", new
                {
                    query,
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<SearchResultModel> SearchAsync(String query, SearchType types, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<SearchResultModel>(
                "/search", new
                {
                    query,
                    types = types.ToString(),
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        #endregion


        #region track methods

        public Task<TrackModel> GetTrackAsync(Int32 trackId)
        {
            return RestClient.HandleAsync<TrackModel>(
                $"/tracks/{trackId}", new
                {
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<ContributorModel>> GetTrackContributorsAsync(Int32 trackId)
        {
            return RestClient.HandleAsync<JsonList<ContributorModel>>(
                $"/tracks/{trackId}/contributors", new
                {
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<TrackModel>> GetRadioFromTrackAsync(Int32 trackId, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<TrackModel>>(
                $"/tracks/{trackId}/radio", new
                {
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        #endregion
    }
}
