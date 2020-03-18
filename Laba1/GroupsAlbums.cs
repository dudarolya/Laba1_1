using System;
using System.Collections.Generic;

namespace Laba1
{
    public partial class GroupsAlbums
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public int GroupId { get; set; }
        public string Info { get; set; }
        public virtual Albums Album { get; set; }
        public virtual Groups Group { get; set; }
    }
}
