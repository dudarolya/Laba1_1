using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laba1
{
    public partial class Albums
    {
        public Albums()
        {
            GroupsAlbums = new HashSet<GroupsAlbums>();
            Songs = new HashSet<Songs>();
        }

        public int AId { get; set; }
        [Required(ErrorMessage = "This field can't be empty!")]
        [Display(Name = "Name of the Album")]
        public string AName { get; set; }
        [Display(Name = "Date of Creation")]
        public DateTime? ACreation { get; set; }

        public virtual ICollection<GroupsAlbums> GroupsAlbums { get; set; }
        public virtual ICollection<Songs> Songs { get; set; }
    }
}
