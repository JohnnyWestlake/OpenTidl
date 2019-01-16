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

using JsonSubTypes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OpenTidl.Models
{
    [DataContract]
    public enum ModuleType
    {
        Unknown,
        FEATURED_PROMOTIONS,
        MULTIPLE_TOP_PROMOTIONS,
        PAGE_LINKS_CLOUD,
        PAGE_LINKS,
        MIX_HEADER,
        MIX_LIST,
        MIXED_TYPES_LIST,
        ARTIST_HEADER,
        ARTIST_LIST,
        ALBUM_LIST,
        PLAYLIST_LIST,
        TRACK_LIST,
        VIDEO_LIST,
        ALBUM_HEADER,
        ALBUM_ITEMS,
        TWITTER,
        FACEBOOK
    }

    [DataContract]
    public class TidalPage
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "rows")]
        public TidalPageRow[] Rows { get; set; }
    }

    [DataContract]
    public class TidalPageRow
    {
        [DataMember(Name = "modules")]
        public TidalModuleBase[] Modules { get; set; }
    }

    
    public interface IModule
    {
        string Type { get; set; }
    }
    
    [DataContract]
    [JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(TidalAlbumHeaderModel), nameof(ModuleType.ALBUM_HEADER))]
    [JsonSubtypes.KnownSubType(typeof(TidalItemsModule), nameof(ModuleType.FEATURED_PROMOTIONS))]
    [JsonSubtypes.KnownSubType(typeof(TidalItemsModule), nameof(ModuleType.MULTIPLE_TOP_PROMOTIONS))]
    [JsonSubtypes.KnownSubType(typeof(TidalModule<TidalMix>), nameof(ModuleType.MIX_LIST))]
    [JsonSubtypes.KnownSubType(typeof(TidalModule<VideoModel>), nameof(ModuleType.VIDEO_LIST))]
    [JsonSubtypes.KnownSubType(typeof(TidalModule<PlaylistModel>), nameof(ModuleType.PLAYLIST_LIST))]
    [JsonSubtypes.KnownSubType(typeof(TidalModule<AlbumModel>), nameof(ModuleType.ALBUM_LIST))]
    [JsonSubtypes.KnownSubType(typeof(TidalModule<ArtistModel>), nameof(ModuleType.ARTIST_LIST))]
    [JsonSubtypes.KnownSubType(typeof(TidalModule<TrackModel>), nameof(ModuleType.TRACK_LIST))]
    [JsonSubtypes.KnownSubType(typeof(TidalModule<TidalPageLink>), nameof(ModuleType.PAGE_LINKS_CLOUD))]
    [JsonSubtypes.KnownSubType(typeof(TidalModule<TidalPageLink>), nameof(ModuleType.PAGE_LINKS))]
    [JsonSubtypes.KnownSubType(typeof(TidalModule<ITidalPlaylistItem>), nameof(ModuleType.ALBUM_ITEMS))]
    public class TidalModuleBase : IModule
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "width")]
        public int Width { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
    }

    [DataContract]
    public class TidalItemsModule : TidalModuleBase
    {
        [DataMember(Name = "items")]
        public TidalModuleItem[] Items { get; set; }
    }

    [DataContract]
    public class TidalModule : TidalModuleBase
    {
        [DataMember(Name = "supportsPaging")]
        public bool SupportsPaging { get; set; }
        [DataMember(Name = "scroll")]
        public string Scroll { get; set; }
        [DataMember(Name = "showMore")]
        public PagingMetadata ShowMore { get; set; }
        [DataMember(Name = "listFormat")]
        public string ListFormat { get; set; }
        [DataMember(Name = "showTableHeaders")]
        public bool ShowTableHeaders { get; set; }
    }

    [DataContract]
    public class TidalModule<T> : TidalModule where T : class
    {
        [DataMember(Name = "pagedList")]
        public TidalPagedList<T> PagedList { get; set; }
    }

    [DataContract]
    public class PagingMetadata
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "apiPath")]
        public string ApiPath { get; set; }
    }

    [DataContract]
    public class TidalPagedList<T> where T : class
    {
        [DataMember(Name = "limit")]
        public int Limit { get; set; }
        [DataMember(Name = "offset")]
        public int Offset { get; set; }
        [DataMember(Name = "totalNumberOfItems")]
        public int TotalNumberOfItems { get; set; }
        [DataMember(Name = "items")]
        public List<T> Items { get; set; }
        [DataMember(Name = "dataApiPath")]
        public string DataApiPath { get; set; }
    }


    [DataContract]
    public class TidalModuleItem
    {
        [DataMember(Name = "header")]
        public string Header { get; set; }
        [DataMember(Name = "shortHeader")]
        public string ShortHeader { get; set; }
        [DataMember(Name = "shortSubHeader")]
        public string ShortSubHeader { get; set; }
        [DataMember(Name = "imageId")]
        public string ImageId { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "artifactId")]
        public string ArtifactId { get; set; }
        [DataMember(Name = "text")]
        public string Text { get; set; }
        [DataMember(Name = "featured")]
        public bool Featured { get; set; }
    }

    [DataContract]
    public class TidalAlbumHeaderModel : TidalModuleBase
    {
        [DataMember(Name = "album")]
        public AlbumModel Album { get; set; }
        [DataMember(Name = "credits")]
        public TidalCreditsModel Credits { get; set; }
        [DataMember(Name = "review")]
        public AlbumReviewModel Review { get; set; }
    }

    [DataContract]
    public class TidalCreditsModel
    {
        [DataMember(Name = "items")]
        public List<CreditModel> Items { get; set; }
    }
}
