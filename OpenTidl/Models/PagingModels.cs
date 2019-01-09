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
    public class PageRoot
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "rows")]
        public Row[] Rows { get; set; }
    }

    [DataContract]
    public class Row
    {
        [DataMember(Name = "modules")]
        public ModuleBase[] Modules { get; set; }
    }

    
    public interface IModule
    {
        string type { get; set; }
    }
    
    [DataContract]
    [JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(ItemsModule), nameof(ModuleType.FEATURED_PROMOTIONS))]
    [JsonSubtypes.KnownSubType(typeof(ItemsModule), nameof(ModuleType.MULTIPLE_TOP_PROMOTIONS))]
    [JsonSubtypes.KnownSubType(typeof(Module<TidalMix>), nameof(ModuleType.MIX_LIST))]
    [JsonSubtypes.KnownSubType(typeof(Module<VideoModel>), nameof(ModuleType.VIDEO_LIST))]
    [JsonSubtypes.KnownSubType(typeof(Module<PlaylistModel>), nameof(ModuleType.PLAYLIST_LIST))]
    [JsonSubtypes.KnownSubType(typeof(Module<AlbumModel>), nameof(ModuleType.ALBUM_LIST))]
    [JsonSubtypes.KnownSubType(typeof(Module<ArtistModel>), nameof(ModuleType.ARTIST_LIST))]
    [JsonSubtypes.KnownSubType(typeof(Module<TrackModel>), nameof(ModuleType.TRACK_LIST))]
    public class ModuleBase : IModule
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember(Name = "width")]
        public int Width { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
    }

    [DataContract]
    public class ItemsModule : ModuleBase
    {
        [DataMember(Name = "items")]
        public ModuleItem[] Items { get; set; }
    }

    [DataContract]
    public class Module<T> : ModuleBase where T : class
    {
        [DataMember(Name = "supportsPaging")]
        public bool SupportsPaging { get; set; }
        [DataMember(Name = "scroll")]
        public string Scroll { get; set; }
        [DataMember(Name = "pagedList")]
        public PagedList<T> PagedList { get; set; }
        [DataMember(Name = "showMore")]
        public PagingMetadata ShowMore { get; set; }
        [DataMember(Name = "listFormat")]
        public string ListFormat { get; set; }
        [DataMember(Name = "showTableHeaders")]
        public bool ShowTableHeaders { get; set; }
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
    public class PagedList<T> where T : class
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
    public class ModuleItem
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
}
