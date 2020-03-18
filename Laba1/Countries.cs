using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laba1
{
    public partial class Countries
    {
        public Countries()
        {
            Artists = new HashSet<Artists>();
        }

        public int CId { get; set; }
        [Required(ErrorMessage = "This field can't be empty!")]
        [Display(Name = "Name of the country")]
        public string CName { get; set; }
        [Required(ErrorMessage = "This field can't be empty!")]
        [Display(Name = "Language of the country")]
        public string CLang { get; set; }
        [Required(ErrorMessage = "This field can't be empty!")]
        [Display(Name = "Capital of the country")]
        public string CCapital { get; set; }

        public virtual ICollection<Artists> Artists { get; set; }
    }
}
