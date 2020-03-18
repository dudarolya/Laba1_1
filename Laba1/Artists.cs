using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laba1
{
    public partial class Artists
    {
        public enum Genders : int
        {
            Female,
            Male,
            Agender,
            Bigender,
            Transgender,
            Transsexual,
            Other
        }
        public int AId { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "This field can't be empty!")]
        public string AName { get; set; }
        [Display(Name = "Gender")]
        [Required(ErrorMessage = "This field can't be empty!")]
        [EnumDataType(typeof(Genders))]
        public Genders AGender { get; set; }
        
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "This field can't be empty!")]
        public DateTime ABirth { get; set; }
        [Display(Name = "Phone")]
        [RegularExpression(@"\d{10}", ErrorMessage = "Invalid phone number!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage ="Invalid number length! It has to be 10 nums")]
        public string APhone { get; set; }
        [Display(Name = "Group Name")]
        [Required(ErrorMessage = "This field can't be empty!")]
        public int GroupId { get; set; }
        [Display(Name = "Country Name")]
        [Required(ErrorMessage = "This field can't be empty!")]
        public int CountryId { get; set; }
        public virtual Countries Country { get; set; }
        public virtual Groups Group { get; set; }
    }
}
