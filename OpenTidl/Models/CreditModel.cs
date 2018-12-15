using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OpenTidl.Models
{
    [DataContract]
    public class CreditModel
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "contributors")]
        public ContributorModel[] Contributors { get; set; }
    }
}
