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
using System.Threading.Tasks;
using OpenTidl.Models;
using OpenTidl.Models.Base;
using OpenTidl.Transport;
using OpenTidl.Enums;

namespace OpenTidl
{
    public partial class OpenTidlClient
    {
        #region image methods

        private static string GetPlaylistImageUrl(String image, String playlistUuid, PlaylistImageSize size)
        {
            int w = 750;
            int h = 500;
            if (!RestUtility.ParseImageSize(size.ToString(), out w, out h))
                throw new ArgumentException("Invalid image size", "size");
            String url = null;
            if (!String.IsNullOrEmpty(image))
                url = String.Format("http://resources.wimpmusic.com/images/{0}/{1}x{2}.jpg", image.Replace('-', '/'), w, h);
            else
                url = String.Format("http://images.tidalhifi.com/im/im?w={1}&h={2}&uuid={0}&rows=2&cols=3&noph", playlistUuid, w, h);
            return url;
        }

        public static string GetAlbumCoverUrl(String cover, Int32 albumId, AlbumCoverSize size)
        {
            int w = 750;
            int h = 750;
            if (!RestUtility.ParseImageSize(size.ToString(), out w, out h))
                throw new ArgumentException("Invalid image size", "size");
            String url = null;
            if (!String.IsNullOrEmpty(cover))
                url = String.Format("http://resources.wimpmusic.com/images/{0}/{1}x{2}.jpg", cover.Replace('-', '/'), w, h);
            else
                url = String.Format("http://images.tidalhifi.com/im/im?w={1}&h={2}&albumid={0}&noph", albumId, w, h);

            return url;
        }

        public static string GetArtistPictureUrl(String picture, Int32 artistId, ArtistPictureSize size)
        {
            int w = 750;
            int h = 500;
            if (!RestUtility.ParseImageSize(size.ToString(), out w, out h))
                throw new ArgumentException("Invalid image size", "size");
            String url = null;
            if (!String.IsNullOrEmpty(picture))
                url = String.Format("http://resources.wimpmusic.com/images/{0}/{1}x{2}.jpg", picture.Replace('-', '/'), w, h);
            else
                url = String.Format("http://images.tidalhifi.com/im/im?w={1}&h={2}&artistid={0}&noph", artistId, w, h);

            return url;
        }

        public static string GetVideoImageUrl(String imageId, String imagePath, VideoImageSize size)
        {
            int w = 750;
            int h = 500;
            if (!RestUtility.ParseImageSize(size.ToString(), out w, out h))
                throw new ArgumentException("Invalid image size", "size");
            String url = null;
            if (!String.IsNullOrEmpty(imageId))
                url = String.Format("http://resources.wimpmusic.com/images/{0}/{1}x{2}.jpg", imageId.Replace('-', '/'), w, h);
            else
                url = String.Format("http://images.tidalhifi.com/im/im?w={1}&h={2}&img={0}&noph", imagePath, w, h);
            return url;
        }

        /// <summary>
        /// Helper method to retrieve a stream with an album cover image
        /// </summary>
        public Task<WebStreamModel> GetAlbumCoverAsync(AlbumModel model, AlbumCoverSize size)
        {
            return GetAlbumCoverAsync(model.Cover, model.Id, size);
        }

        /// <summary>
        /// Helper method to retrieve a stream with an album cover image
        /// </summary>
        public Task<WebStreamModel> GetAlbumCoverAsync(String cover, Int32 albumId, AlbumCoverSize size)
        {
            string url = GetAlbumCoverUrl(cover, albumId, size);
            return RestClient.GetWebStreamModelAsync(url);
        }

        /// <summary>
        /// Helper method to retrieve a stream with an artists picture
        /// </summary>
        public Task<WebStreamModel> GetArtistPictureAsync(ArtistModel model, ArtistPictureSize size)
        {
            return GetArtistPictureAsync(model.Picture, model.Id, size);
        }

        /// <summary>
        /// Helper method to retrieve a stream with an artists picture
        /// </summary>
        public Task<WebStreamModel> GetArtistPictureAsync(String picture, Int32 artistId, ArtistPictureSize size)
        {
            string url = GetArtistPictureUrl(picture, artistId, size);
            return RestClient.GetWebStreamModelAsync(url);
        }

        /// <summary>
        /// Helper method to retrieve a stream with a playlist image
        /// </summary>
        public Task<WebStreamModel> GetPlaylistImageAsync(PlaylistModel model, PlaylistImageSize size)
        {
            return GetPlaylistImageAsync(model.Image, model.Uuid, size);
        }

        /// <summary>
        /// Helper method to retrieve a stream with a playlist image
        /// </summary>
        public Task<WebStreamModel> GetPlaylistImageAsync(String image, String playlistUuid, PlaylistImageSize size)
        {
            string url = GetPlaylistImageUrl(image, playlistUuid, size);
            return RestClient.GetWebStreamModelAsync(url);
        }

        /// <summary>
        /// Helper method to retrieve a stream with a video conver image
        /// </summary>
        public Task<WebStreamModel> GetVideoImageAsync(VideoModel model, VideoImageSize size)
        {
            return GetVideoImageAsync(model.ImageId, model.ImagePath, size);
        }

        /// <summary>
        /// Helper method to retrieve a stream with a video conver image
        /// </summary>
        public Task<WebStreamModel> GetVideoImageAsync(String imageId, String imagePath, VideoImageSize size)
        {
            string url = GetVideoImageUrl(imageId, imagePath, size);
            return RestClient.GetWebStreamModelAsync(url);
        }

        #endregion

        #region track/video methods

        /// <summary>
        /// Helper method to retrieve the audio/video stream with correct user-agent, etc.
        /// </summary>
        public Task<WebStreamModel> GetWebStreamAsync(String streamUrl)
        {
            return RestClient.GetWebStreamModelAsync(streamUrl);
        }

        #endregion
    }
}
