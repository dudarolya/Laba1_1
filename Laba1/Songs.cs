using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laba1
{
    public partial class Songs
    {
        public int SId { get; set; }
        [Required(ErrorMessage = "This field can't be empty!")]
        [Display(Name = "Name")]
        public string SName { get; set; }
        [Required(ErrorMessage = "This field can't be empty!")]
        [Display(Name = "Duration of the song")]
        public TimeSpan SDuration { get; set; }
        [Required(ErrorMessage = "This field can't be empty!")]
        [Display(Name = "Album Name")]
        public int AlbumId { get; set; }
        [Required(ErrorMessage = "This field can't be empty!")]
        [Display(Name = "Genre Name")]
        public int GenreId { get; set; }

        public virtual Albums Album { get; set; }
        public virtual Genres Genre { get; set; }
    }
}
