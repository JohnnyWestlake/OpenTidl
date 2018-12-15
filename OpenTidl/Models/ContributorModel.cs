﻿/*
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
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTidl.Models.Base;
using OpenTidl.Enums;
using System.Runtime.Serialization;
using OpenTidl.Transport;

namespace OpenTidl.Models
{
    [DataContract]
    public class ContributorModel : ModelBase
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public String Name { get; private set; }

        [DataMember(Name = "type")]
        public String Type { get; private set; }

        [IgnoreDataMember]
        public ContributorRole Role { get; private set; }


        #region enum helpers

        [DataMember(Name = "role")]
        private String RoleEnumHelper
        {
            get { return Role.ToString(); }
            set { Role = RestUtility.ParseEnum<ContributorRole>(value); }
        }

        #endregion
    }
}
