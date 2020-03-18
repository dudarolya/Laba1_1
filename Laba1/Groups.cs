using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laba1
{
    public partial class Groups
    {
        public Groups()
        {
            Artists = new HashSet<Artists>();
            GroupsAlbums = new HashSet<GroupsAlbums>();
        }

        public int GrId { get; set; }
        [Required(ErrorMessage = "This field can't be empty!")]
        [Display(Name = "Name of the group")]
        public string GrName { get; set; }
        [Display(Name = "Date of creation")]
        public DateTime? GrCreation { get; set; }

        public virtual ICollection<Artists> Artists { get; set; }
        public virtual ICollection<GroupsAlbums> GroupsAlbums { get; set; }
    }
}
