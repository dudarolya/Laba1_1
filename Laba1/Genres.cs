using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laba1
{
    public partial class Genres
    {
        public Genres()
        {
            Songs = new HashSet<Songs>();
        }

        public int GenId { get; set; }
        [Required(ErrorMessage = "This field can't be empty!")]
        [Display(Name = "Name of the genre")]
        public string GenName { get; set; }

        public virtual ICollection<Songs> Songs { get; set; }
    }
}
