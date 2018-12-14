using OpenTidl.Models.Base;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OpenTidl.Models
{
    [DataContract]
    public class AssetModel : ModelBase
    {
        [DataMember(Name = "urls")]
        public string[] Urls { get; set; }

        [DataMember(Name = "trackId")]
        public int TrackId { get; set; }

        [DataMember(Name = "assetPresentation")]
        public string AssetPresentation { get; set; }

        [DataMember(Name = "audioQuality")]
        public string AudioQuality { get; set; }

        [DataMember(Name = "streamingSessionId")]
        public string StreamingSessionId { get; set; }

        [DataMember(Name = "codec")]
        public string Codec { get; set; }
    }

}
