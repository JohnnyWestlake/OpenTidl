using OpenTidl.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OpenTidl.Models
{
    [DataContract]
    public class FavoritesModel : ModelBase
    {
        [DataMember(Name = "PLAYLIST")]
        public HashSet<string> Playlists { get; private set; }

        [DataMember(Name = "TRACK")]
        public HashSet<string> Tracks { get; private set; }

        [DataMember(Name = "VIDEO")]
        public HashSet<string> Video { get; private set; }

        [DataMember(Name = "ALBUM")]
        public HashSet<string> Albums { get; private set; }

        [DataMember(Name = "ARTIST")]
        public HashSet<string> Artists { get; private set; }
    }
}
