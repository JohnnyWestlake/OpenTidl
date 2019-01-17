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
