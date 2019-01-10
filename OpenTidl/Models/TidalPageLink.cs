using OpenTidl.Models.Base;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OpenTidl.Models
{
    [DataContract]
    public class TidalPageLink : ModelBase
    {
        [DataMember(Name = "apiPath")]
        public String ApiPath { get; private set; }

        [DataMember(Name = "icon")]
        public String Icon { get; private set; }

        [DataMember(Name = "imageId")]
        public String ImageId { get; private set; }

        [DataMember(Name = "title")]
        public String Title { get; private set; }
    }
}
