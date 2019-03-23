using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTidl.Models
{
    public class UserPlaylistModel
    {
        public DateTime Created { get; set; }
        public PlaylistModel Playlist { get; set; }
        public UserPlaylistType Type {get;set;}
    }

    public enum UserPlaylistType
    {
        USER_CREATED,
        USER_FAVORITE
    }
}
