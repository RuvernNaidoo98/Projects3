using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sprint33.Models
{
    public class Patient
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please use letters only")]
        [Display(Name ="Name")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please use letters only")]
        [Display(Name ="Surname")]
        public string Surname { get; set; }
        [Required]
        [Display(Name ="Age")]
        public int Age { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please use numbers only")]
        [Display(Name ="Contact Number")]

        public int ContactNumber { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name ="Patient`s Active Status")]
        public bool isActive { get; set; }
        

    }
}