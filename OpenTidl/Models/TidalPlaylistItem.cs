using JsonSubTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OpenTidl.Models
{
    [JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(TidalPlaylistTrackItem), "track")]
    [JsonSubtypes.KnownSubType(typeof(TidalPlaylistVideoItem), "video")]
    public interface ITidalPlaylistItem
    {
        [DataMember(Name="type")]
        string Type { get; set; }
    }

    [DataContract]
    public class TidalPlaylistTrackItem : ITidalPlaylistItem
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "item")]
        public TrackModel Item { get; set; }
    }

    [DataContract]
    public class TidalPlaylistVideoItem : ITidalPlaylistItem
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "item")]
        public VideoModel Item { get; set; }
    }
}
