using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.Models
{
    public class SignUpUserModel
    {
        [Required(ErrorMessage = "Please enter a First Name")]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }
        [Required(ErrorMessage = "Please enter a Last Name")]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "Please Select your date of birth")]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }


        //[Required(ErrorMessage = "Please enter a Username")]
       // [Display(Name = "User Name")]
       // public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Phone Number")]
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Please enter a valid phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter a strong Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
