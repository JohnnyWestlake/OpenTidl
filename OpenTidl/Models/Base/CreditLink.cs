using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OpenTidl.Models.Base
{
    [DataContract]
    public class CreditLink
    {
        [DataMember(Name ="item")]
        public TrackModel Item { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "credits")]
        public CreditModel[] Credits { get; set; }
    }
}
